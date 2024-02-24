using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace PaintballWorld.Infrastructure
{
    public class Constants
    {
        private static readonly string BasePath = "C:\\Files";

        public static string RegulationsPath = Path.Combine(BasePath, "Regulations");
        public static string FieldPhotosPath = Path.Combine(BasePath, "FieldPhotos");
    }
}
