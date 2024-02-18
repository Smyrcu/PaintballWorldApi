using PaintballWorld.API.Areas.Field.Models;

namespace PaintballWorld.API.Areas.Auth.Models
{
    public class OwnerWithFieldDto : OwnerDto
    {
        public FieldDto Field { get; set; }
        public OwnerWithFieldDto(string email, string username, string password, string firstName, string lastName, DateTime dateOfBirth, CompanyDto company, FieldDto field) : base(email, username, password, firstName, lastName, dateOfBirth, company)
        {
            this.Field = field;
        }
    }
}
