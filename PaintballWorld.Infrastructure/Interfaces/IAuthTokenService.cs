using Microsoft.AspNetCore.Identity;

namespace PaintballWorld.Infrastructure.Interfaces;

public interface IAuthTokenService
{
    string GenerateToken(IdentityUser user);
}