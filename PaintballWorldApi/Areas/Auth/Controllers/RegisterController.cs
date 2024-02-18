using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaintballWorld.API.Areas.Auth.Data;
using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;

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
        private readonly ApplicationDbContext _context;
        private readonly IProfileService _profileService;
        private readonly IOwnerService _ownerService;
        private readonly IOwnerRegistrationService _ownerRegistrationService;
        private readonly IFieldManagementService _fieldManagementService;

        #endregion

        #region Konstruktor

        public RegisterController(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IEmailService emailService, 
            IConfiguration configuration, 
            ILogger<LoginController> logger, ApplicationDbContext context, IProfileService profileService, IOwnerService ownerService, IOwnerRegistrationService ownerRegistrationService, IFieldManagementService fieldManagementService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
            _context = context;
            _profileService = profileService;
            _ownerService = ownerService;
            _ownerRegistrationService = ownerRegistrationService;
            _fieldManagementService = fieldManagementService;
        }

        #endregion

        #region Rejestracja
        
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var user = new IdentityUser
            {
                UserName = userDto.Username,
                Email = userDto.Email,
            };
            _logger.LogInformation("Creating new account");

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                var confirmationGuid = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmAccount", "Register",
                    new { Area = "Auth", userId = user.Id, code = confirmationGuid },
                    protocol: HttpContext.Request.Scheme);

                await _emailService.SendConfirmationEmailAsync(user.Email, callbackUrl);

                _profileService.FinishRegistration(user, userDto.DateOfBirth);

                await _userManager.AddToRoleAsync(user, "User");

                return Ok("User was created successfully");
            }
            return BadRequest(result);
        }

        [HttpPost]
        [Route("RegisterOwner")]
        public async Task<IActionResult> RegisterOwner([FromBody] OwnerDto dto)
        {
            var user = new IdentityUser
            {
                UserName = dto.Username,
                Email = dto.Email,
            };
            _logger.LogInformation("Creating new OwnerDto account");

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                var confirmationGuid = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmAccount", "Register",
                    new { Area = "Auth", userId = user.Id, code = confirmationGuid },
                    protocol: HttpContext.Request.Scheme);

                await _emailService.SendConfirmationEmailAsync(user.Email, callbackUrl);

                _profileService.FinishRegistration(user, dto.DateOfBirth, dto.FirstName, dto.LastName);

                _ownerRegistrationService.RegisterOwner(dto.Map(user));

                await _userManager.AddToRoleAsync(user, "Owner");

                return Ok("User was created successfully");
            }

            return BadRequest(result);

        }

        [HttpPost]
        [Route("RegisterOwnerWithField")]
        public async Task<IActionResult> RegisterOwnerWithField([FromBody] OwnerWithFieldDto dto)
        {
            var user = new IdentityUser
            {
                UserName = dto.Username,
                Email = dto.Email,
            };
            _logger.LogInformation("Creating new OwnerDto account");

            var result = await _userManager.CreateAsync(user, dto.Password);
            
            if (result.Succeeded)
            {
                var confirmationGuid = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmAccount", "Register",
                    new { Area = "Auth", userId = user.Id, code = confirmationGuid },
                    protocol: HttpContext.Request.Scheme);

                await _emailService.SendConfirmationEmailAsync(user.Email, callbackUrl);

                _profileService.FinishRegistration(user, dto.DateOfBirth, dto.FirstName, dto.LastName);

                var fieldTypeId = _fieldManagementService.GetFieldTypeIdByStringName(dto.Field.FieldType);

                var map = dto.Map(user, fieldTypeId);
                _ownerRegistrationService.RegisterOwnerWithField(map.Item1, map.Item2);

                await _userManager.AddToRoleAsync(user, "Owner");

                return Ok("User with field was created successfully");
            }

            return BadRequest(result);

        }

        [HttpGet]
        [Route("Test")]
        public IActionResult Test()
        {
            return Ok(JsonConvert.SerializeObject(new OwnerWithFieldDto("test", "test", "test", "test", "test",
                DateTime.Now,
                new CompanyDto("test", "test", "test",
                    new AddressDto("test", "test", "test", "test", "test", "test", "test")), 
                new FieldDto(new AddressDto("test", "test", "test", "test", "test", "test", "test"), null, 5000, "test", "test", "test",10, 100, 10, "paintball"))));
        }

        #endregion



        #region Potwierdzanie konta

        [HttpGet("ConfirmAccount")]
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
