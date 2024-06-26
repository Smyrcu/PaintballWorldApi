using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PaintballWorld.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Org.BouncyCastle.Asn1.X509;
using PaintballWorld.API;
using PaintballWorld.API.Filters;
using PaintballWorld.API.Middleware;
using System.Reflection;
using NetTopologySuite.IO.Converters;


Directory.CreateDirectory("C:\\Files");

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ProdConnection") 
                       ?? throw new InvalidOperationException("Connection string 'ProdConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, provider =>
    {
        provider.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
        provider.UseNetTopologySuite();
    });
    options.LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//DI
builder.Services.Inject();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new NetTopologySuite.IO.Converters.GeometryConverter());
    options.SerializerSettings.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueCountLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = int.MaxValue;
});


builder.Services.AddLogging(logger =>
{
    logger.ClearProviders();
    logger.SetMinimumLevel(LogLevel.Trace);
    logger.AddConsole();
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Configuration not found")))
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = "525733446611-mrmhd13qf4ob46svf14grbo02njfc9fl.apps.googleusercontent.com";
        options.ClientSecret = "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";
        // options.CallbackPath = new PathString("/Authentication/Register/GoogleResponse");
        options.CallbackPath = new PathString("/signin-google");
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "PaintballWorldApi", Version = "v1" });

    c.SchemaFilter<GeoPointSchemaFilter>(); 
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Constants.BasePath),
    RequestPath = "/img"
});

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Constants.BasePath),
    RequestPath = "/regulations"
});

app.UseCors("AllowAll");

app.UseRouting();

app.UseMiddleware<ApiKeyMiddleware>();


app.UseAuthentication(); 
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

// Migracje
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
context.Database.Migrate();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "area",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.MapControllers();

app.Run();
