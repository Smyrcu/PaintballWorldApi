using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Field.Data;
using PaintballWorld.Core.Interfaces;

namespace PaintballWorld.API.Areas.Field.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Field")]
    [AllowAnonymous]
    public class FieldsController : ControllerBase
    {
        private readonly IFieldManagementService _fieldManagementService;


        public FieldsController(IFieldManagementService fieldManagementService)
        {
            _fieldManagementService = fieldManagementService;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateField([FromBody] FieldDto fieldDto)
        {
            if (fieldDto.OwnerId is null)
                return BadRequest("Nie podano Id Ownera");

            var fieldTypeId = _fieldManagementService.GetFieldTypeIdByStringName(fieldDto.FieldType);

            _fieldManagementService.CreateField(fieldDto.Map(fieldTypeId));
            return Ok("Field was created successfully");
        }
    }
}
