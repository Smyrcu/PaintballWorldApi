﻿namespace PaintballWorld.Infrastructure.Models;

public readonly record struct CompanyId(Guid Value)
{
    public static CompanyId Empty => new(Guid.Empty);
    public static CompanyId NewCompanyId() => new(Guid.NewGuid());
}
public partial class Company
{
    public CompanyId Id { get; init; }// = CompanyId.Empty;
    public AddressId AddressId { get; set; }
    public virtual Address Address { get; set; }
    public string? CompanyName { get; set; } = null!;
    public string? TaxId { get; set; }

    public string? Email { get; set; }

}
