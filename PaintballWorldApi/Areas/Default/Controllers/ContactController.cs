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

        [HttpPost]
        public async Task<IActionResult> Contact([FromBody] ContactDto dto)
        {
            await _contactService.SaveContactMessage(dto.Email, dto.Content, dto.Title);
            return Ok();
        }
    }
}
