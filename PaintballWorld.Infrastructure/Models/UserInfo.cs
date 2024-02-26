using Microsoft.AspNetCore.Identity;

namespace PaintballWorld.Infrastructure.Models;

public partial class UserInfo
{
    public string UserId { get; set; }
    public virtual IdentityUser User { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Description { get; set; }

    public PhotoId? ProfileImageId { get; set; }
    public Photo? ProfileImage { get; set; }

    public string? PhoneNo { get; set; }

    public virtual ICollection<UserRating> Ratings { get; set; }
}
