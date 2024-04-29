using System.Globalization;
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
        /// <summary>
        /// Do sprawdzania czy api żyje / czasem testuje tu coś xd
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Ping")]
        public Task<IActionResult> Ping()
        {

            var cultureInfo = CultureInfo.CurrentCulture;


            return Task.FromResult<IActionResult>(Ok("I'm alive! " + cultureInfo.Name));
        }
    }
}
