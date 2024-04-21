using Microsoft.AspNetCore.Identity;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Interfaces;

public interface IOwnerService
{
    public void Approve();
    FieldId GetFieldId(OwnerId owner);
}