using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintballWorld.Core.Models
{
    public class CompanyModel
    {
        public string? TaxId { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public AddressModel Address { get; set; }
    }
}
