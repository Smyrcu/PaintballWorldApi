using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintballWorld.Infrastructure.Models
{
    public readonly record struct ContactId(Guid Value)
    {
        public static ContactId Empty => new(Guid.Empty);
        public static ContactId NewContactId() => new(Guid.NewGuid());
    }
    public class Contact
    {
        public ContactId Id { get; init; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string Topic { get; set; }
    }
}
