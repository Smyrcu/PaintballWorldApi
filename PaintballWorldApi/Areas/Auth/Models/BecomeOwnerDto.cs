namespace PaintballWorld.API.Areas.Auth.Models
{
    public class BecomeOwnerDto
    {
        public BecomeOwnerDto( string firstName, string lastName, CompanyDto company)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Company = company;
        }


        public string FirstName { get; init; }
        public string LastName { get; init; }
        public CompanyDto Company { get; init; }

    }
}
