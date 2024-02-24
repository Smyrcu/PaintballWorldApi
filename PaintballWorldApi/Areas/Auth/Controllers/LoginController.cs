﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PaintballWorld.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using PaintballWorld.API.Areas.Auth.Models;

namespace PaintballWorld.API.Areas.Auth.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Auth")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        #region Properties

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;
        private readonly IAuthTokenService _tokenService;

        #endregion

        #region Konstruktor
        public LoginController(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IEmailService emailService, 
            IConfiguration configuration, 
            ILogger<LoginController> logger, 
            IAuthTokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
            _tokenService = tokenService;
        }
        #endregion

        #region Account

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user == null)
                return NotFound("User not found");

            var result = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized(result);

            var jwtToken = await _tokenService.GenerateToken(user);

            return jwtToken == string.Empty
                ? Unauthorized(result)
                : Ok(new { Token = jwtToken });

        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Route("DeleteAccount")]
        [HttpPost]
        public async Task<IActionResult> DeleteAccount([FromBody] string username)
        {
            _logger.Log(LogLevel.Information, $"Szukam usera do usunięcia usename: {username}");
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
            {
                _logger.Log(LogLevel.Warning, $"Nie znalazłem usera: {username}");
                return NotFound();
            }
            _logger.Log(LogLevel.Information, $"Użytkownik {username} znaleziony - usuwam");
            try
            {
                await _userManager.DeleteAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Usuwanie usera {username} spadło z rowerka");
                throw;
            }
            return Ok();
        }

        #endregion

        #region Password

        /// <summary>
        /// Wysyła email i linkiem do zmiany hasła
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [Route("ResetPasswordRequest")]
        public async Task<IActionResult> ResetPasswordRequest([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action("ResetPassword", "Login",
                new { Area = "Auth", userId = user.Id, token = token },
                protocol: HttpContext.Request.Scheme);
            if (callbackUrl is not null)
            {
                await _emailService.SendResetPasswordEmailAsync(email, callbackUrl);
                return Ok();
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resetowanie hasła
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return NotFound();
            }

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        #endregion

    }
}
