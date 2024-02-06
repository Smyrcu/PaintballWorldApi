using System;
using System.Collections.Generic;

namespace PaintballWorld.Infrastructure.Models;

public partial class UserInfo
{
    public Guid UserId { get; set; }

    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string? Image { get; set; }

    public string? PhoneNo { get; set; }
}
