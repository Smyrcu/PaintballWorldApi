using Microsoft.AspNetCore.Identity;

namespace PaintballWorld.Infrastructure.Interfaces;

public interface IAuthTokenService
{
    Task<string> GenerateToken(IdentityUser user);
}