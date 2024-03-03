using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.BaseModels;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure.Interfaces;

namespace PaintballWorld.API.Areas.User.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IAuthTokenService _authTokenService;

    public UserController(IUserService userService, IAuthTokenService authTokenService)
    {
        _userService = userService;
        _authTokenService = authTokenService;
    }


    [HttpGet("profile")]
    public IActionResult ShowProfile()
    {
        try
        {
            var userId = _authTokenService.GetUserId(User.Claims);

            var result = _userService.GetUserInfo(userId);
            return Ok(result);

        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseBase
            {
                Errors = [ex.Message],
                IsSuccess = false
            });
        }
    }
}