using Microsoft.AspNetCore.Identity;

namespace PaintballWorld.Core.Interfaces;

public interface IProfileService
{
    void FinishRegistration(IdentityUser user, DateTime dateOfBirth);
}