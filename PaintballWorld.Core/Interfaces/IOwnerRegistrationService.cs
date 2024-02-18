using Google.Apis.Util.Store;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Interfaces;

public interface IOwnerRegistrationService
{
    public void RegisterOwner(Owner owner);
    public void RegisterOwnerWithField(Owner owner, Field field);
}