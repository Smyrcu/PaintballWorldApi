using System.Drawing;
using System.Drawing.Imaging;
using Google.Apis.Gmail.v1.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite;
using Org.BouncyCastle.Bcpg.OpenPgp;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Core.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;


namespace PaintballWorld.Core.Services
{
    public class FieldManagementService : IFieldManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        private const int SRID = 4326;

        public FieldManagementService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public void CreateField(Field field)
        {
            _context.Fields.Add(field);
            _context.SaveChanges();
        }

        public FieldTypeId GetFieldTypeIdByStringName(string fieldType) =>
            _context.FieldTypes.Single(x => x.FieldTypeName.Equals(fieldType)).Id;

        public string SaveRegulationsFile(Stream stream, FieldId fieldId)
        {
            var filename = GetRegulationsFileName(fieldId);
            Directory.CreateDirectory(Constants.RegulationsPath); // TODO: Tworzyć te ścieżki przy starcie apki
            var path = _fileService.SaveFile(Path.Combine(Constants.RegulationsPath, filename), stream);

            return path.Replace(Constants.BasePath + "\\", "");
        }

        public string SavePhoto(Stream stream, FieldId fieldId)
        {
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(GetPhotoFileName(fieldId));
            var directory = Path.Combine(Constants.FieldPhotosPath, fieldId.Value.ToString());
            Directory.CreateDirectory(directory);

            var originalFilePath = Path.Combine(directory, $"{filenameWithoutExtension}.jpg");
            var thumbnailFilePath = Path.Combine(directory, $"{filenameWithoutExtension}.300.jpg");

            using var image = Image.FromStream(stream);

            image.Save(originalFilePath, ImageFormat.Jpeg);

            using var thumbnail = ResizeImage(image, 300);
            thumbnail.Save(thumbnailFilePath, ImageFormat.Jpeg);

            var savePath = string.Join("/",Constants.FieldPhotosPath.Replace(Constants.BasePath + "\\", ""), fieldId.Value.ToString(), $"{filenameWithoutExtension}.jpg");

            var photo = new Photo
            {
                Path = savePath,
                FieldId = fieldId,
                EventId = null,
                CreatedOnUtc = DateTime.Now
            };

            _context.Photos.Add(photo);
            _context.SaveChanges();

            return originalFilePath;
        }

        public void SaveChanges(Field updRecord)
        {
            var field = _context.Fields.Include(field => field.Address).FirstOrDefault(x => x.Id == updRecord.Id);
            if (field == null)
                throw new Exception("Field not found");
            
            field.Address.City = updRecord.Address.City;
            field.Address.Country = updRecord.Address.Country;
            field.Address.Location = updRecord.Address.Location;
            field.Address.HouseNo = updRecord.Address.HouseNo;
            field.Address.Street = updRecord.Address.Street;
            field.Address.PhoneNo = updRecord.Address.PhoneNo;
            field.Address.PostalNumber = updRecord.Address.PostalNumber;
            field.Area = updRecord.Area;
            field.Name = updRecord.Name;
            field.Description = updRecord.Description;
            field.MinPlayers = updRecord.MinPlayers;
            field.MaxPlayers = updRecord.MaxPlayers;
            field.Sets = updRecord.Sets;
            field.MaxSimultaneousEvents = updRecord.MaxSimultaneousEvents;

            _context.Fields.Update(field);
            _context.SaveChanges();

        }

        public async Task<List<FilteredField>> GetFieldsFiltered(OsmCityId osmCityId, decimal filterRadius)
        {
            var city = await _context.OsmCities
                .Where(c => c.Id == osmCityId)
                .Select(c => new { c.Latitude, c.Longitude })
                .FirstOrDefaultAsync();

            if (city == null)
            {
                return new ();
            }

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(SRID);
            var cityLocation = geometryFactory.CreatePoint(new Coordinate((double)city.Longitude, (double)city.Latitude));

            var radiusInMeters = (double)filterRadius * 1000d;

            var fieldsInRadius = await _context.Fields
                .Where(f => f.Address.Location.IsWithinDistance(cityLocation, radiusInMeters))
                .Select(f => new FilteredField
                {
                    FieldId = f.Id,
                    FieldName = f.Name,
                    CityName = f.Address.City,
                    GeoPoint = f.Address.Location
                })
                .ToListAsync();

            return fieldsInRadius;
        }

        private static string GetRegulationsFileName(FieldId fieldId) => $"Reg_{fieldId.Value}_{DateTime.Now:yyyy_MM_dd}.pdf";

        private string GetPhotoFileName(FieldId fieldId)
        {
            var count = _context.Photos.Count(x => x.FieldId == fieldId);
            return $"Photo_{fieldId.Value}_{count}.png";

            /*var maxNumber = photos.Select(photo => int.TryParse(photo.Path.Split('_').LastOrDefault(), out var number) ? number : 0).Max();

            return $"Photo_{fieldId.Value}_{maxNumber++}.png";*/

        }

        private static Image ResizeImage(Image image, int size)
        {
            var ratio = (double)size / (image.Width > image.Height ? image.Width : image.Height);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using var g = Graphics.FromImage(newImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

    }
}
