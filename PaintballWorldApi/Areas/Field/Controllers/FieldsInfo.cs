using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Field.Models;

namespace PaintballWorld.API.Areas.Field.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Field")]
    [AllowAnonymous]
    public class FieldsInfo : Controller
    {
        private readonly ILogger<FieldsInfo> logger;

        // [HttpGet]
        // [Route("GetField/{fieldId}")]
        // public async Task<IActionResult<FieldInfoDto>> GetFieldInfo(Guid fieldId)
        // {
        //     return default;
        // }





    }
}
