using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Default.Models;
using PaintballWorld.Infrastructure.Interfaces;

namespace PaintballWorld.API.Areas.Default.Controllers
{
    [Route("api/[Area]/[controller]")]
    [Area("Default")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Zapisanie wiadomości "kontakt"
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Contact([FromBody] ContactDto dto)
        {
            await _contactService.SaveContactMessage(dto.Email, dto.Content, dto.Title);
            return Ok();
        }
    }
}
