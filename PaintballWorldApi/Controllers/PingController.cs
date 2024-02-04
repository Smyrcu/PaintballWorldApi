using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PaintballWorld.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("/api/[controller]")]
    public class PingController : Controller
    {
        [HttpGet]
        [Route("Ping")]
        public Task<IActionResult> Ping()
        {
            return Task.FromResult<IActionResult>(Ok("I'm alive!"));
        }
    }
}
