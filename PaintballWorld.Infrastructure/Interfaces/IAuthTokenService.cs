using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Infrastructure.Interfaces;

public interface IAuthTokenService
{
    Task<string> GenerateToken(IdentityUser user);
    (bool, string) IsUserOwnerOfField(IEnumerable<Claim> userClaims, FieldId id);
}