using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PaintballWorld.Infrastructure;

namespace PaintballWorld.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("/api/[controller]")]
    public class PingController(ILogger<PingController> logger, ApplicationDbContext context) : Controller
    {
        private readonly ILogger<PingController> logger = logger;
        private readonly ApplicationDbContext context = context;

        [HttpGet]
        [Route("Ping")]
        public Task<IActionResult> Ping()
        {
            logger.LogWarning("test");
            string connectionString = "Server=tcp:paintballazuresql.database.windows.net,1433;Initial Catalog=CountForPW;Persist Security Info=False;User ID=pwadmin;Password=1q2w3e$R;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection connection = new(connectionString);
            connection.Open();
            var users = context.Users.Count();
            var fields = context.Fields.Count();
            var owners = context.Owners.Count();

           //connection.Execute("CREATE TABLE CountsTable(Name nvarchar(20), Count int)");

            //var sql = @"INSERT INTO CountsTable (Name, Count) VALUES('Owners', @owners)";

            //connection.Execute(sql, new { owners });

            var sqlUsers = "UPDATE ct SET ct.Count = @users FROM CountsTable ct WHERE ct.Name = 'Users'";
            var sqlFields = "UPDATE ct SET ct.Count = @fields FROM CountsTable ct WHERE ct.Name = 'Fields'";
            var sqlOwners = "UPDATE ct SET ct.Count = @owners FROM CountsTable ct WHERE ct.Name = 'Owners'";

            connection.Execute(sqlUsers, new { users });
            connection.Execute(sqlFields, new { fields });
            connection.Execute(sqlOwners, new { owners });

            NLog.LogManager.Flush();
            return Task.FromResult<IActionResult>(Ok("I'm alive!"));
        }
    }
}
