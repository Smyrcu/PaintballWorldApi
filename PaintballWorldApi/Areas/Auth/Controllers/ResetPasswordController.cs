using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.Infrastructure.Interfaces;
using System.Security.Claims;

namespace PaintballWorld.API.Areas.Auth.Controllers
{
    [Route("api/[Area]/[controller]")]
    [Area("Auth")]
    [ApiController]
    public class ResetPasswordController(UserManager<IdentityUser> userManager, IEmailService emailService, IAuthTokenService tokenService)
        : ControllerBase
    {
        #region Password

        /// <summary>
        /// Wysyła email i linkiem do zmiany hasła
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<IActionResult> ResetPasswordRequest([FromQuery] string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            return Ok(new { /*userId = user.Id,*/ token = token});
        }
        // https://www.TwojaStara.com/Reset/Password?token=XXX&?UserId-123876129
        /// <summary>
        /// Resetowanie hasła
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return NotFound();
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, dto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new ResetPasswordResponse
            {
                IsSuccess = true,
                Errors = [],
                Message = "New password has been set"
            });
        }

        /// <summary>
        /// Zmiana hasła
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var username = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (username is null)
            {
                return Unauthorized("Wrong JWT");
            }

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var checkOldPasswordResult = await userManager.CheckPasswordAsync(user, dto.OldPassword);
            if (!checkOldPasswordResult)
            {
                return BadRequest("Old Password is incorrect");
            }

            var result = await userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password was changed");
        }


        #endregion

    }
}
