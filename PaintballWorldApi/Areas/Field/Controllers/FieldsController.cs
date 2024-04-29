using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Field.Data;
using PaintballWorld.Core.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class FieldsController(
        IFieldManagementService fieldManagementService,
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        IAuthTokenService authTokenService,
        ILogger<FieldsController> logger)
        : ControllerBase
    {
        private const int SRID = 4326;

        /// <summary>
        /// Utwórz pole
        /// </summary>
        /// <param name="fieldDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateField([FromForm] FieldDto fieldDto)
        {
            try
            {
                var username = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)
                    ?.Value;

                if (username == null)
                    return BadRequest("Wrong JWT");

                var user = await userManager.FindByNameAsync(username);

                if (user == null)
                    return BadRequest("User not found");

                var owner = context.Owners.FirstOrDefault(x => x.UserId == Guid.Parse(user.Id));

                // W zasadzie Authorize powinno to filtrować
                // Ale nie filtruje HMMM
                if (owner == null)
                    return BadRequest(
                        "This account is not Owner");

                fieldDto.OwnerId = owner?.Id;

                var fieldTypeId = fieldManagementService.GetFieldTypeIdByStringName(fieldDto.FieldType);

                var mapped = fieldDto.Map(fieldTypeId);

                if (fieldDto.Regulations is not null)
                {
                    using var stream = new MemoryStream();
                    await fieldDto.Regulations.CopyToAsync(stream);
                    mapped.Regulations = fieldManagementService.SaveRegulationsFile(stream, mapped.Id);
                }

                fieldManagementService.CreateField(mapped);

                var additionalText = owner.IsApproved ? "" : " - Owner is not approved!";

                return Ok($"Field was created successfully{additionalText}");
            }
            catch (Exception ex)
            {
                // return Forbid();
                return BadRequest(ex.Message);

            }
        }
        
        /// <summary>
        /// pobierz dane pola
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        [HttpGet("{fieldId}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetField([FromRoute]Guid fieldId)
        {
            var id = new FieldId(fieldId);

            var field = context.Fields.Include(x => x.Address)
                .Include(x => x.Sets).Include(x => x.FieldType).Include(x => x.Owner).ThenInclude(x => x.Company).FirstOrDefault(x => x.Id == id);

            if (field == null)
                return BadRequest("Field not found");

            /*var isOwner = _authTokenService.IsUserOwnerOfField(User.Claims, id);
            if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
                return BadRequest(isOwner.errors);*/

            var urlPrefix = $"{Request.Scheme}://{Request.Host}";

            var result = field.Map(urlPrefix);

            result.OwnerName = field.Owner.Company.CompanyName;

            return Ok(result);
        }

        /// <summary>
        /// pobierz pola (filtrowanie)
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFieldsFiltered([FromQuery] FieldFilters filter)
        {
            var result = await fieldManagementService.GetFieldsFiltered(new OsmCityId(filter.Id), filter.Radius);

            var model = result.Map().ToList();

            return Ok(model);
        }

        /// <summary>
        /// edytuj dane pola
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        [HttpPut("{fieldId}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> ManageField([FromForm]FieldManagementDto dto, [FromRoute]Guid fieldId)
        {

            var isOwner = authTokenService.IsUserOwnerOfField(User.Claims, new FieldId(fieldId));
            if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
                return BadRequest(isOwner.errors);
            try
            {
                fieldManagementService.SaveChanges(dto.Map());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Nie udało się zapisać zmian");
                return BadRequest("Updating data failed");
            }

            return Ok("Data saved successfully");

        }

        /// <summary>
        /// usuń pole
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        [HttpDelete("{fieldId}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteField([FromRoute] Guid fieldId)
        {
            var fieldIdModel = new FieldId(fieldId);
            var isOwner = authTokenService.IsUserOwnerOfField(User.Claims, fieldIdModel);
            if (!isOwner.success || !isOwner.errors.IsNullOrEmpty())
            {
                return BadRequest(new AddSetsResponse
                {
                    Errors = [ isOwner.errors ],
                    IsSuccess = false,
                    Message = "Owner not found"
                });
            }

            var field = await context.Fields.FindAsync(fieldIdModel);
            if (field == null)
            {
                return BadRequest(new ResponseBase
                {
                    IsSuccess = false,
                    Errors = ["Field not found"],
                });
            }

            context.Fields.Remove(field);
            await context.SaveChangesAsync();
            return Ok(new ResponseBase
            {
                IsSuccess = true,
                Message = "Field deleted successfully"
            });

        }
    }
}
