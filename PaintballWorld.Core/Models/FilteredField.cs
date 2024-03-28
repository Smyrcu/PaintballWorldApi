using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Models
{
    public class FilteredField
    {
        public FieldId FieldId { get; set; }
        public string? FieldName { get; set; }
        public string? CityName { get; set; }
    }
}
