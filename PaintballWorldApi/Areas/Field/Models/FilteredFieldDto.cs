using NetTopologySuite.Geometries;

namespace PaintballWorld.API.Areas.Field.Models
{
    public class FilteredFieldDto
    {
        public Guid FieldId { get; set; }
        public string? FieldName { get; set; }
        public string? CityName { get; set; }
        public Point? GeoPoint { get; set; }
    }
}
