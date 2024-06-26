﻿namespace PaintballWorld.Infrastructure.Models;

public readonly record struct FieldId(Guid Value)
{
    public static FieldId Empty => new(Guid.Empty);
    public static FieldId NewFieldId() => new(Guid.NewGuid());
}

public partial class Field
{
    public FieldId Id { get; init; }// = FieldId.Empty;
    public FieldTypeId FieldTypeId { get; set; }
    public virtual FieldType FieldType { get; set; }
    public  AddressId AddressId { get; set; }
    public virtual Address Address { get; set; }
    public OwnerId OwnerId { get; set; }
    public virtual Owner Owner { get; set; }
    public double Area { get; set; }

    public string? Name { get; set; }

    public string? Regulations { get; set; }

    public string? Description { get; set; }

    public int MinPlayers { get; set; }

    public int MaxPlayers { get; set; }

    public virtual PhotoId? MainPhotoId { get; set; }
    public virtual Photo? MainPhoto { get; set; }

    public virtual ICollection<Set> Sets { get; set; }

    public int MaxSimultaneousEvents { get; set; }

    public DateTime LastUpdatedUtc { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
    public virtual ICollection<FieldRating> FieldRatings { get; set; }
    public virtual ICollection<FieldSchedule> FieldSchedules { get; set; }
    public virtual ICollection<Photo> Photos { get; set; }
    public virtual ICollection<Event> Events { get; set; }
}


