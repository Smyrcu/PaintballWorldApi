namespace PaintballWorld.API.Areas.Auth.Models
{
    public class CompanyDto
    {
        public CompanyDto(string? taxId, string? companyName, string? email, AddressDto address)
        {
            this.TaxId = taxId;
            this.CompanyName = companyName;
            this.Email = email;
            this.Address = address;
        }

        public string? TaxId { get; init; }
        public string? CompanyName { get; init; }
        public string? Email { get; init; }
        public AddressDto Address { get; init; }

        public void Deconstruct(out string? TaxId, out string? CompanyName, out string? Email, out AddressDto Address)
        {
            TaxId = this.TaxId;
            CompanyName = this.CompanyName;
            Email = this.Email;
            Address = this.Address;
        }
    }
}
