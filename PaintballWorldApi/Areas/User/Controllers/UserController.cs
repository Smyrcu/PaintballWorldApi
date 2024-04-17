using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.User.Data;
using PaintballWorld.API.Areas.User.Models;
using PaintballWorld.API.BaseModels;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure.Interfaces;

namespace PaintballWorld.API.Areas.User.Controllers;

[Route("api/[area]/[controller]")]
[ApiController]
[Area("User")]
[AllowAnonymous]
public class UserController(IUserService userService, IAuthTokenService authTokenService)
    : Controller
{
    [HttpGet("profile")]
    public IActionResult ShowProfile()
    {
        try
        {
            var userId = authTokenService.GetUserId(User.Claims);

            var result = userService.GetUserInfo(userId);

            var model = result.Map();
            
            return Ok(model);

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

    [HttpPut("profile")]
    public IActionResult UpdateProfile([FromBody] UserProfileDto dto)
    {
        try
        {
            var userId = authTokenService.GetUserId(User.Claims);

            userService.UpdateProfile(dto.Map(userId));
            return Ok(new ResponseBase
            {
                IsSuccess = true
            });
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