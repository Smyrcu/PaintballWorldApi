
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using PaintballWorld.Infrastructure;
using System.Net.Mail;
using System.Net;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
using CsvHelper.Configuration;
using CsvHelper;
using System.Data;
using Dapper;

//var host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices((context, services) =>
//    {
//        // Tutaj dodajemy nasze serwisy do kontenera DI
//        services.AddScoped<IFileService, FileService>();
//        services.AddScoped<IEmailService, EmailService>();
//        services.AddScoped<IAuthTokenService, AuthTokenService>();

//        services.AddDbContext<ApplicationDbContext>();

//        services.AddLogging(logger =>
//        {
//            logger.ClearProviders();
//            logger.SetMinimumLevel(LogLevel.Trace);
//            logger.AddConsole();
//        });
//    })
//    .Build();

//// Zdefiniuj, jak użyć serwisu emailowego
//async Task RunEmailService(IHost host)
//{
//    var emailService = host.Services.GetRequiredService<IEmailService>();

//    // Tutaj używamy serwisu do wysłania emaila
//    await emailService.SendEmailAsync("blszadkowski@gmail.com", "Test Email", "This is a test email body.");
//}

//// Uruchamiamy naszą aplikację
//await RunEmailService(host);
// using System;
// using System.Net;
// using System.Net.Mail;
// using System.Threading.Tasks;
//
// string smtpServer = "smtp.gmail.com"; // Adres serwera SMTP Gmail
// int smtpPort = 587; // Port SMTP dla Gmail
// string smtpUsername = "paintballworldpw@gmail.com"; // Twój adres e-mail Gmail
// string smtpPassword = "ugyd uwlg vlxe xkos"; // Hasło do konta Gmail
//
// string recipientEmail = "bl.szadkowski@gmail.com"; // Adres e-mail odbiorcy
// string subject = "Temat wiadomości";
// string body = "Treść wiadomości";
//
// using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
// {
//     client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
//     client.EnableSsl = true; // Wyłącz SSL
//
//     MailMessage message = new MailMessage(smtpUsername, recipientEmail, subject, body);
//
//     try
//     {
//         await client.SendMailAsync(message);
//         Console.WriteLine("Wiadomość została wysłana pomyślnie.");
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine("Wystąpił błąd podczas wysyłania wiadomości: " + ex.Message);
//     }
// }

var connectionString = "Server=127.0.0.1,9210;User Id=sa;Password=JakiesLosoweHaslo123;Database=PaintballWorldApp2;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=false";
var csvFilePath = @"C:\Users\User\Desktop\OsmCities.csv";
// var csvFilePath = @"C:\Test\OsmCities.csv";

var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
     Delimiter = ",",
     HasHeaderRecord = true,
};

using var reader = new FileStream(csvFilePath, FileMode.Open, FileAccess.Read);
using var sr = new StreamReader(reader);
using var csv = new CsvReader(sr, config);
using var connection = new System.Data.SqlClient.SqlConnection(connectionString);
connection.Open();

var records = ReadCsv<OsmCity>(csvFilePath, x => x.Id = Guid.NewGuid()).ToList();

BulkInsert(connection, records, "OsmCities");

static IEnumerable<T> ReadCsv<T>(string filePath, Action<T> action)
    where T : class
{
    var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

    var ci = CultureInfo.InvariantCulture;

    CsvConfiguration conf = new(ci);

    conf.HasHeaderRecord = true;
    conf.Delimiter = ",";

    var reader = new StreamReader(stream);
    var csv = new CsvReader(reader, conf);
    while (csv.Read())
    {
        var record = csv.GetRecord<T>();
        action.Invoke(record);
        yield return record;
    }
}

static void BulkInsert<T>(System.Data.SqlClient.SqlConnection connection, IEnumerable<T> data,
    string targetTableName)
{
    using (System.Data.SqlClient.SqlCommand cmd = new(
               $"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{targetTableName}') BEGIN CREATE TABLE {targetTableName} ({string.Join(", ", typeof(T).GetProperties().Select(p => $"{p.Name} NVARCHAR(MAX)"))}) END",
               connection))
    {
        cmd.ExecuteNonQuery();
    }

    // Wstawianie danych do tabeli
    using System.Data.SqlClient.SqlBulkCopy bulkCopy = new(connection);
    bulkCopy.DestinationTableName = targetTableName;
    using var reader = new ObjectDataReader<T>(data);
    foreach (var property in typeof(T).GetProperties())
    {
        bulkCopy.ColumnMappings.Add(property.Name, property.Name);
    }

    bulkCopy.WriteToServer(reader);
}


// foreach (var record in records)
// {
//     var commandText = @"INSERT INTO dbo.OsmCities (Id, OsmId, Latitude, Longitude, Name, County, Municipality, Province, PostalCode) 
//                             VALUES (NEWID(), @OsmId, @Latitude, @Longitude, @Name, @County, @Municipality, @Province, @PostalCode)";
//
//     using var command = new SqlCommand(commandText, connection);
//     command.Parameters.AddWithValue("@OsmId", record.OsmId);
//     command.Parameters.AddWithValue("@Latitude", record.Latitude);
//     command.Parameters.AddWithValue("@Longitude", record.Longitude);
//     command.Parameters.AddWithValue("@Name", record.Name);
//     command.Parameters.AddWithValue("@County", record.County);
//     command.Parameters.AddWithValue("@Municipality", record.Municipality);
//     command.Parameters.AddWithValue("@Province", record.Province);
//     command.Parameters.AddWithValue("@PostalCode", record.PostalCode);
//
//     command.ExecuteNonQuery();
// }
//
public class OsmCity
{
    [Ignore]
    public Guid Id { get; set; }
    public long OsmId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    [Name("Name")]
    public string Name { get; set; }
    [Name("County")]
    public string County { get; set; }
    [Name("Municipality")]
    public string Municipality { get; set; }
    [Name("Province")]
    public string Province { get; set; }
    [Name("PostalCode")]
    public string PostalCode { get; set; }
}

public class ObjectDataReader<T> : IDataReader
{
    private readonly IEnumerator<T> _enumerator;
    private readonly List<System.Reflection.PropertyInfo> _properties;

    public ObjectDataReader(IEnumerable<T> data)
    {
        _enumerator = data.GetEnumerator();
        _properties = typeof(T).GetProperties().ToList();
    }

    public object GetValue(int i)
    {
        return _properties[i].GetValue(_enumerator.Current);
    }

    public int GetValues(object[] values)
    {
        throw new NotImplementedException();
    }

    public int FieldCount => _properties.Count;

    public bool Read()
    {
        return _enumerator.MoveNext();
    }

    public void Close()
    {
        Dispose();
    }

    public void Dispose()
    {
        _enumerator.Dispose();
    }

    public int GetOrdinal(string name)
    {
        return _properties.FindIndex(p => p.Name == name);
    }

    public bool IsDBNull(int i)
    {
        return GetValue(i) == null;
    }

    public string GetName(int i)
    {
        return _properties[i].Name;
    }

    public string GetDataTypeName(int i)
    {
        return _properties[i].PropertyType.Name;
    }

    public Type GetFieldType(int i)
    {
        return _properties[i].PropertyType;
    }

    public int Depth => 0;

    public bool IsClosed => _enumerator == null;

    public int RecordsAffected => -1;

    public bool NextResult()
    {
        return false;
    }

    public DataTable GetSchemaTable()
    {
        var schemaTable = new DataTable();

        foreach (var prop in typeof(T).GetProperties())
        {
            var column = new DataColumn
            {
                ColumnName = prop.Name,
                DataType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType
            };
            schemaTable.Columns.Add(column);
        }

        return schemaTable;
    }

    public object this[int i] => GetValue(i);

    public object this[string name] => GetValue(GetOrdinal(name));

    public bool GetBoolean(int i)
    {
        return (bool)GetValue(i);
    }

    public byte GetByte(int i)
    {
        return (byte)GetValue(i);
    }

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
        throw new NotImplementedException();
    }

    public char GetChar(int i)
    {
        return (char)GetValue(i);
    }

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
        throw new NotImplementedException();
    }

    public Guid GetGuid(int i)
    {
        return (Guid)GetValue(i);
    }

    public short GetInt16(int i)
    {
        return (short)GetValue(i);
    }

    public int GetInt32(int i)
    {
        return (int)GetValue(i);
    }

    public long GetInt64(int i)
    {
        return (long)GetValue(i);
    }

    public float GetFloat(int i)
    {
        return (float)GetValue(i);
    }

    public double GetDouble(int i)
    {
        return (double)GetValue(i);
    }

    public string GetString(int i)
    {
        return (string)GetValue(i);
    }

    public decimal GetDecimal(int i)
    {
        return (decimal)GetValue(i);
    }

    public DateTime GetDateTime(int i)
    {
        return (DateTime)GetValue(i);
    }

    public IDataReader GetData(int i)
    {
        throw new NotImplementedException();
    }
}