using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Default.Models;

namespace PaintballWorld.API.Areas.Default.Controllers
{
    [Route("api/[Area]/[controller]")]
    [Area("Default")]
    [ApiController]
    public class ContactController : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> Contact([FromBody] ContactDto dto)
        {
            

        }
    }
}
