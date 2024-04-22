using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaintballWorld.API.Areas.Auth.Data;
using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure;

namespace PaintballWorld.API.Areas.Auth.Controllers
{
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Area("Auth")]
    public class RegisterOwnerController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IEmailService emailService,
        IConfiguration configuration,
        ILogger<RegisterOwnerController> logger,
        ApplicationDbContext context,
        IProfileService profileService,
        IOwnerService ownerService,
        IOwnerRegistrationService ownerRegistrationService,
        IFieldManagementService fieldManagementService,
        IAuthTokenService authTokenService)
        : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly ApplicationDbContext _context = context;
        private readonly IOwnerService _ownerService = ownerService;
        private readonly IFieldManagementService _fieldManagementService = fieldManagementService;
        private readonly IAuthTokenService _authTokenService = authTokenService;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        /// <summary>
        /// Rejestracja nowego użytkownika jako ownera pola
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterOwner([FromBody] OwnerDto dto)
        {
            var user = new IdentityUser
            {
                UserName = dto.Username,
                Email = dto.Email,
            };
            logger.LogInformation("Creating new OwnerDto account");

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                var confirmationGuid = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmAccount", "Register",
                    new { Area = "Auth", userId = user.Id, code = confirmationGuid },
                    protocol: HttpContext.Request.Scheme);

                await emailService.SendConfirmationEmailAsync(user.Email, callbackUrl);

                profileService.FinishRegistration(user, dto.DateOfBirth, dto.FirstName, dto.LastName);

                ownerRegistrationService.RegisterOwner(dto.Map(user));

                await _userManager.AddToRoleAsync(user, "Owner");

#if DEBUG
                return Ok(callbackUrl);
#endif

                return Ok("User was created successfully");

            }

            return BadRequest(result);

        }

        /// <summary>
        /// Upgrade konta z usera do ownera
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpgradeToOwner([FromBody] BecomeOwnerDto dto)
        {
            var userId = _authTokenService.GetUserId(User.Claims);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                //profileService.FinishRegistration(user, dto.DateOfBirth, dto.FirstName, dto.LastName);

                ownerRegistrationService.RegisterOwner(dto.Map(user));

                await _userManager.AddToRoleAsync(user, "Owner");

                return Ok("Account upgraded successfully");
            }

            return BadRequest("User not found");

        }
    }
}
