using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Field.Data;
using PaintballWorld.Core.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PaintballWorld.API.Areas.Field.Models;
using PaintballWorld.API.BaseModels;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;

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
        private readonly IAuthTokenService _authTokenService;
        private readonly ILogger<FieldsController> _logger;

        private const int SRID = 4326;
        public FieldsController(IFieldManagementService fieldManagementService, ApplicationDbContext context, UserManager<IdentityUser> userManager, IAuthTokenService authTokenService, ILogger<FieldsController> logger)
        {
            _fieldManagementService = fieldManagementService;
            _context = context;
            _userManager = userManager;
            _authTokenService = authTokenService;
            _logger = logger;
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
                // Ale nie filtruje HMMM
                if (owner == null)
                    return BadRequest(
                        "This account is not Owner");

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
        
        [HttpGet("{fieldId}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetField([FromRoute]Guid fieldId)
        {
            var id = new FieldId(fieldId);

            var field = _context.Fields.FirstOrDefault(x => x.Id == id);

            if (field == null)
                return BadRequest("Field not found");

            /*var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims, id);
            if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
                return BadRequest(isOwner.errors);*/

            var result = field.Map();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFieldsFiltered([FromQuery] FieldFilters filter)
        {
            var result = await _fieldManagementService.GetFieldsFiltered(new OsmCityId(filter.Id), filter.Radius);

            return Ok(result);
        }

        [HttpPut("{fieldId}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> ManageField([FromForm]FieldManagementDto dto, [FromRoute]Guid fieldId)
        {

            var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims, new FieldId(fieldId));
            if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
                return BadRequest(isOwner.errors);
            try
            {
                _fieldManagementService.SaveChanges(dto.Map());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Nie udało się zapisać zmian");
                return BadRequest("Updating data failed");
            }

            return Ok("Data saved successfully");

        }

        [HttpDelete("{fieldId}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteField([FromRoute] Guid fieldId)
        {
            var fieldIdModel = new FieldId(fieldId);
            var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims, fieldIdModel);
            if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
            {
                return BadRequest(new AddSetsResponse
                {
                    Errors = [ isOwner.errors ],
                    IsSuccess = false,
                    Message = "Owner not found"
                });
            }

            var field = await _context.Fields.FindAsync(fieldIdModel);
            if (field == null)
            {
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Errors = ["Field not found"],
                });
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();
            return Ok(new ResponseBase
            {
                IsSuccess = true,
                Message = "Field deleted successfully"
            });

        }
    }
}
