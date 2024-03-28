using Microsoft.OpenApi.Models;
using NetTopologySuite.Geometries;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PaintballWorld.API.Filters
{
    public class GeoPointSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(Point))
            {
                schema.Type = "object";
                schema.Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["latitude"] = new OpenApiSchema
                    {
                        Type = "number",
                        Format = "double"
                    },
                    ["longitude"] = new OpenApiSchema
                    {
                        Type = "number",
                        Format = "double"
                    }
                };
            }
        }
    }
}
