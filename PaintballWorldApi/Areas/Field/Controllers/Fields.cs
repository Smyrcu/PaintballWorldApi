using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Field.Data;
using PaintballWorld.Core.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using PaintballWorld.Infrastructure;

namespace PaintballWorld.API.Areas.Field.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Field")]
    [AllowAnonymous]
    public class FieldsController : ControllerBase
    {
        private readonly IFieldManagementService _fieldManagementService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;



        public FieldsController(IFieldManagementService fieldManagementService, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _fieldManagementService = fieldManagementService;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateField([FromForm] FieldDto fieldDto)
        {
            try
            {
                var username = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)
                    ?.Value;

                if (username == null)
                    return BadRequest("Wrong JWT");

                var user = await _userManager.FindByNameAsync(username);

                if (user == null)
                    return BadRequest("User not found");

                var owner = _context.Owners.FirstOrDefault(x => x.UserId == Guid.Parse(user.Id));

                // W zasadzie Authorize powinno to filtrować 
                if (owner == null)
                    return BadRequest(
                        "This account is not Owner - Jak tu trafiłeś daj znać bo nie powinno tego hitować nigdy");

                fieldDto.OwnerId = owner?.Id;

                var fieldTypeId = _fieldManagementService.GetFieldTypeIdByStringName(fieldDto.FieldType);

                var mapped = fieldDto.Map(fieldTypeId);

                if (fieldDto.Regulations is not null)
                {
                    using var stream = new MemoryStream();
                    await fieldDto.Regulations.CopyToAsync(stream);
                    mapped.Regulations = _fieldManagementService.SaveRegulationsFile(stream, mapped.Id);
                }

                _fieldManagementService.CreateField(mapped);

                var additionalText = owner.IsApproved ? "" : " - Owner is not approved!";

                return Ok($"Field was created successfully{additionalText}");
            }
            catch (Exception ex)
            {
                // return Forbid();
                return BadRequest(ex.Message);

            }
        }
    }
}
