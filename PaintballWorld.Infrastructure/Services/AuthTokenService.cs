using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.X9;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Infrastructure.Services
{
    public class AuthTokenService : IAuthTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AuthTokenService(IConfiguration configuration, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> GenerateToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetUserId(IEnumerable<Claim> userClaims)
        {
            var username = userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (username == null)
            {
                throw new Exception("Wrong JWT");
            }

            var user = _userManager.FindByNameAsync(username).GetAwaiter().GetResult();
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user.Id;

        } 

        public (bool success, string errors) IsUserOwnerOfField(IEnumerable<Claim> userClaims, FieldId id)
        {
            var error = "";
            var username = userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (username == null)
            {
                error = "Wrong JWT";
                return (false, error);
            }

            var user = _userManager.FindByNameAsync(username).GetAwaiter().GetResult();
            if (user == null)
            {
                error = "User not found";
                return (false, error);
            }

            var owner = _context.Owners.FirstOrDefault(x => x.UserId == Guid.Parse(user.Id));

            if (owner == null)
            {
                error = "This account is not Owner - Jak tu trafiłeś daj znać bo nie powinno tego hitować nigdy";
                return (false, error);
            }

            var fieldIds = _context.Fields.FirstOrDefault(x => x.OwnerId == owner.Id && x.Id == id);

            if (fieldIds != null) return (true, error);
            error = "This user is not the Owner of this field.";
            return (false, error);

        }


    }
}
