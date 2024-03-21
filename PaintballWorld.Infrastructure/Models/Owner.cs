namespace PaintballWorld.Infrastructure.Models;

public readonly record struct OwnerId(Guid Value)
{
    public static OwnerId Empty => new(Guid.Empty);
    public static OwnerId NewOwnerId() => new(Guid.NewGuid());
}


public partial class Owner
{
    public OwnerId Id { get; init; }// = OwnerId.Empty;
    public Guid UserId { get; set; }

    public CompanyId CompanyId { get; set; }
    public virtual Company Company { get; set; }
    public virtual ICollection<Field> Fields { get; set; }
    public bool IsApproved { get; set; }

    public DateTime LastUpdatedUtc { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
