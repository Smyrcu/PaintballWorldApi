namespace PaintballWorld.API.Areas.Auth.Models
{
    public record RegisterRequest(string Email, string Username, string Password, DateTime DateOfBirth);

    public record RegisterOwner(string FirstName, string LastName, DateTime DateOfBirth, RegisterCompany Company);

    public record RegisterCompany(string? TaxId, string? CompanyName, string? PhoneNo, string? Email,
        RegisterAddress Address);

    public record RegisterAddress(string? PhoneNo, string? Street, string? HouseNo, string? City, string? PostalNumber,
        string? Country, string? Coordinates);

    public record RegisterOwnerRequest(string Email, string Username, string Password, string? PhoneNumber, RegisterOwner Owner);

}
