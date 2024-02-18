using Microsoft.AspNetCore.Identity;

namespace PaintballWorld.Infrastructure.Models;

public readonly record struct UserRatingId(Guid Value)
{
    public static UserRatingId Empty => new(Guid.Empty);
    public static UserRatingId NewEventId() => new(Guid.NewGuid());
}


public class UserRating
{
    public UserRatingId Id { get; init; } = UserRatingId.Empty;

    public string UserId { get; set; }
    public virtual UserInfo User { get; set; }

    public double Rating { get; set; }

    public string? Content { get; set; }

    public string? CreatorId { get; set; }

    public virtual IdentityUser? Creator { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
}
