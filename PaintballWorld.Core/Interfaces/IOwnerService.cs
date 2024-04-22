using Microsoft.AspNetCore.Identity;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Interfaces;

public interface IOwnerService
{
    public void Approve();
    Guid? GetFieldId(string userId);
}