using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Infrastructure.Interfaces;

public interface IAuthTokenService
{
    Task<string> GenerateToken(IdentityUser user);
    (bool success, string errors) IsUserOwnerOfField(IEnumerable<Claim> userClaims, FieldId id);
    string GetUserId(IEnumerable<Claim> userClaims);
}