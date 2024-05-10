namespace PaintballWorld.Infrastructure.Models;

public readonly record struct AttachmentId(Guid Value)
{
    public static AttachmentId Empty => new(Guid.Empty);
    public static AttachmentId NewEventId() => new(Guid.NewGuid());
}

public partial class Attachment
{
    public AttachmentId Id { get; init; }// = AttachmentId.Empty;
    public EmailInboxId? EmailInboxId { get; set; }
    // public virtual EmailInbox? EmailInbox { get; Set; }
    public EmailOutboxId? EmailOutboxId { get; set; }
    // public virtual EmailOutbox? EmailOutbox { get; Set; }
    public bool EmailType { get; set; }

    public string? Path { get; set; }
}
