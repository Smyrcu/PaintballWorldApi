using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintballWorld.Infrastructure.Models
{

    public readonly record struct ApiKeyId(Guid Value)
    {
        public static ApiKeyId Empty => new(Guid.Empty);
        public static ApiKeyId NewEventId() => new(Guid.NewGuid());
    }
    public class ApiKey
    {
        public ApiKeyId Id { get; private set; } = ApiKeyId.Empty;
        public string Key { get; set; }

        public string Name { get; set; }
    }
}
