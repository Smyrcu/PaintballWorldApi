using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintballWorld.Infrastructure.Models
{
    public class GeoPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GeoPoint(double longitude, double latitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public GeoPoint()
        {
            
        }
    }
}
