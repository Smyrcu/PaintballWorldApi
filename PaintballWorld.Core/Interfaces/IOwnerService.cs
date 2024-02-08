using Microsoft.AspNetCore.Identity;
using PaintballWorld.Core.Models;

namespace PaintballWorld.Core.Interfaces;

public interface IOwnerService
{
    void RegisterOwner(IdentityUser user, OwnerModel request);
}