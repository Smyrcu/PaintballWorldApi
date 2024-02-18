using Microsoft.AspNetCore.Identity;

namespace PaintballWorld.Core.Interfaces;

public interface IProfileService
{
    void FinishRegistration(IdentityUser user, DateTime dateOfBirth);
    void FinishRegistration(IdentityUser user, DateTime dateOfBirth, string firstName, string lastName);
}