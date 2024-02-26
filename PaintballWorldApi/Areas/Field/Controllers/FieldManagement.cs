using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using PaintballWorld.API.Areas.Field.Data;
using PaintballWorld.API.Areas.Field.Models;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;

namespace PaintballWorld.API.Areas.Field.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Field")]
    [AllowAnonymous]
    public class FieldManagement : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFieldManagementService _fieldManagementService;
        private readonly IAuthTokenService _authTokenService;
        private readonly ILogger<FieldManagement> _logger;


        public FieldManagement(IFieldManagementService fieldManagementService, UserManager<IdentityUser> userManager, ApplicationDbContext context, IAuthTokenService authTokenService, ILogger<FieldManagement> logger)
        {
            this._fieldManagementService = fieldManagementService;
            _userManager = userManager;
            _context = context;
            _authTokenService = authTokenService;
            _logger = logger;
        }


        [HttpPost("AddFieldPhotos")]
        [Authorize(Roles = "Owner")]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> AddPhotos([FromForm]IFormFileCollection photos, [FromForm]Guid fieldId)
        {
            var username = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (username == null)
                return BadRequest("Wrong JWT");

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return BadRequest("User not found");

            var owner = _context.Owners.FirstOrDefault(x => x.UserId == Guid.Parse(user.Id));

            if(owner == null)
                return BadRequest("This account is not Owner - Jak tu trafiłeś daj znać bo nie powinno tego hitować nigdy");
            var parsedFieldId = new FieldId(fieldId);

            var fieldIds = _context.Fields.FirstOrDefault(x => x.OwnerId == owner.Id && x.Id == parsedFieldId);
            
            if(fieldIds == null)
                return BadRequest("This user is not the Owner of this field.");

            var errors = new List<string>();

            foreach (var photo in photos)
            {
                if (photo.Length > 0)
                {
                    var extension = Path.GetExtension(photo.FileName).ToLower();
                    if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                    {
                        errors.Add($"{photo.FileName} - Invalid file type. Only JPG and PNG are allowed.");
                    }
                    else
                    {
                        try
                        {
                            using var stream = new MemoryStream();
                            await photo.CopyToAsync(stream);
                            stream.Position = 0;
                            var fId = new FieldId(fieldId);
                            _fieldManagementService.SavePhoto(stream, fId);
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Error uploading {photo.FileName}: {ex.Message}");
                        }
                    }
                }
                else
                {
                    errors.Add($"{photo.FileName} is empty and cannot be uploaded.");
                }
            }
            var message = errors.Any()
                ? "Photos uploaded with errors"
                : "Photos uploaded successfully.";

            return Ok(new { Message = message, Errors = errors});
        }


        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> ManageField([FromQuery]Guid fieldId)
        {
            var id = new FieldId(fieldId);

            var field = _context.Fields.FirstOrDefault(x => x.Id == id);

            if (field == null)
                return BadRequest("Field not found");

            var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims, id);
            if (!isOwner.Item1 || !isOwner.Item2.IsNullOrEmpty())
                return BadRequest(isOwner.Item2);

            var result = field.Map();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> ManageField([FromForm]FieldManagementDto dto)
        {

            var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims, dto.FieldId);
            if (!isOwner.Item1 || !isOwner.Item2.IsNullOrEmpty())
                return BadRequest(isOwner.Item2);
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



    }
}
