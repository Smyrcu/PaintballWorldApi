using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PaintballWorld.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PaintballWorld.API;
using PaintballWorld.API.Middleware;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ProdConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, provider =>
    {
        provider.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
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

builder.Services.AddControllers();

builder.Services.AddLogging(logger =>
{

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
builder.Services.AddSwaggerGen();

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

app.UseCors("AllowAll");

app.UseRouting();

app.UseMiddleware<ApiKeyMiddleware>();


app.UseAuthentication(); 
app.UseAuthorization();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();

    //var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //context.Database.Migrate();

//}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "area",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.MapControllers();

app.Run();
