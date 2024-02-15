using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PaintballWorld.Infrastructure.Models;

public partial class UserInfo
{
    public string UserId { get; set; }
    public virtual IdentityUser User { get; set; }

    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string? Image { get; set; }

    public string? PhoneNo { get; set; }

    public virtual ICollection<UserRating> Ratings { get; set; }
}
