using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.Infrastructure.Interfaces;
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

        /// <summary>
        /// Logowanie użytkownika
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user == null)
                return NotFound("User not found");

            var result = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized(result);

            var jwtToken = await _tokenService.GenerateToken(user);

            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.Contains("Owner") ? "Owner" : "User";

            return jwtToken == string.Empty
                ? Unauthorized(result)
                : Ok(new { Token = jwtToken, Role = role  });

        }

        /// <summary>
        /// Wylogowywanie użytkownika / Ubijanie tokena
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }


        /// <summary>
        /// Usuwanie konta
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteUserDto dto)
        {
            _logger.Log(LogLevel.Information, $"Szukam usera do usunięcia usename: {dto.Username}");
            var user = await _userManager.FindByNameAsync(dto.Username);
             
            if (user is null)
            {
                _logger.Log(LogLevel.Warning, $"Nie znalazłem usera: {dto.Username}");
                return NotFound();
            }
            _logger.Log(LogLevel.Information, $"Użytkownik {dto.Username} znaleziony - usuwam");
            try
            {
                await _userManager.DeleteAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Usuwanie usera {dto.Username} spadło z rowerka");
                throw;
            }
            return Ok();
        }

        #endregion

    }
}
