using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Interfaces;

public interface IOwnerRegistrationService
{
    public void RegisterOwner(Owner owner);
}