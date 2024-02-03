using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NLog.Extensions.Logging;
using PaintballWorld.Infrastructure;
using System.Globalization;
using PaintballWorld.API;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//DI
builder.Services.Inject();

builder.Services.AddControllers();

builder.Services.AddLogging(logger =>
{

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "area",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
