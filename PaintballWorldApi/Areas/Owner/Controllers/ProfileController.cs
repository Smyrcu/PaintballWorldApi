using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Owner.Models;
using PaintballWorld.API.BaseModels;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.API.Areas.Owner.Controllers
{
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Area("Owner")]
    public class ProfileController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly IAuthTokenService _authTokenService;


        public ProfileController(IOwnerService ownerService, IAuthTokenService authTokenService)
        {
            _ownerService = ownerService;
            _authTokenService = authTokenService;
        }

        /// <summary>
        /// Pobierz Id pola dla aktualnie zalogowanego ownera
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMyFieldId()
        {
            var userId = _authTokenService.GetUserId(User.Claims);
            var fieldId = _ownerService.GetFieldId(new OwnerId(Guid.Parse(userId)));
            return Ok(new MyFieldsResponseModel
            {
                IsSuccess = true,
                FieldId = fieldId
            });
        }
    }
}
