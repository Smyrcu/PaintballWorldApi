using PaintballWorld.API.Areas.Auth.Models;
using PaintballWorld.Core.Models;

namespace PaintballWorld.API.Areas.Auth.Data
{
    public static class OwnerModelMapper
    {
        public static OwnerModel Map(this RegisterOwner registerOwner)
        {
            return new OwnerModel
            {
                FirstName = registerOwner.FirstName,
                LastName = registerOwner.LastName,
                DateOfBirth = registerOwner.DateOfBirth,
                Company = new CompanyModel
                {
                    TaxId = registerOwner.Company.TaxId,
                    CompanyName = registerOwner.Company.CompanyName,
                    PhoneNo = registerOwner.Company.PhoneNo,
                    Email = registerOwner.Company.Email,
                    Address = new AddressModel
                    {
                        PhoneNo = registerOwner.Company.Address.PhoneNo,
                        Street = registerOwner.Company.Address.Street,
                        HouseNo = registerOwner.Company.Address.HouseNo,
                        City = registerOwner.Company.Address.City,
                        PostalNumber = registerOwner.Company.Address.PostalNumber,
                        Country = registerOwner.Company.Address.Country,
                        Coordinates = registerOwner.Company.Address.Coordinates
                    }
                }
            };




        }



    }
}
