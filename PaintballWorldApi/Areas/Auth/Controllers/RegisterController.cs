using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.Infrastructure.Interfaces;

namespace PaintballWorld.API.Areas.Auth.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Auth")]
    [AllowAnonymous]
    public class RegisterController : ControllerBase
    {

        #region Properties

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        #endregion

        #region Konstruktor

        public RegisterController(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IEmailService emailService, 
            IConfiguration configuration, 
            ILogger<LoginController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        #endregion

        #region Rejestracja

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
            };
            _logger.LogInformation("Creating new account", user);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var confirmationGuid = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmAccount", "Register",
                    new { Area = "Auth", userId = user.Id, code = confirmationGuid },
                    protocol: HttpContext.Request.Scheme);

                await _emailService.SendConfirmationEmailAsync(user.Email, callbackUrl);
                return Ok();
            }
            return BadRequest(result);
        }

        #endregion

        #region Potwierdzanie konta

        [HttpPost("ConfirmAccount")]
        public async Task<IActionResult> ConfirmAccount([FromQuery] string userId, [FromQuery] string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return BadRequest("Nieprawidłowy identyfikator użytkownika lub kod.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound("Nie znaleziono użytkownika");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Ok("Adres e-mail został potwierdzony");
            }
            else
            {
                return BadRequest("Błąd podczas potwierdzania adresu e-mail.");
            }
        }

        #endregion
    }
}
