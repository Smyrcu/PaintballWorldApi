using PaintballWorld.API.Areas.Auth.Models;

namespace PaintballWorld.API.Areas.Auth.Models
{
    public class OwnerDto : UserDto
    {
        public OwnerDto(string? email, string username, string? password, string firstName, string lastName, DateTime dateOfBirth, CompanyDto company) : base(email, username, password, dateOfBirth)
        {
            this.Email = email;
            this.Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Company = company;
        }


        public string FirstName { get; init; }
        public string LastName { get; init; }
        public CompanyDto Company { get; init; }

        public void Deconstruct(out string firstName, out string lastName, out DateTime dateOfBirth, out CompanyDto company)
        {
            firstName = this.FirstName;
            lastName = this.LastName;
            dateOfBirth = this.DateOfBirth;
            company = this.Company;
        }
    }
}