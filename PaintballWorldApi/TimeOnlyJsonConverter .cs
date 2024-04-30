using Newtonsoft.Json;

namespace PaintballWorld.API
{

    public class TimeOnlyJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeOnly) || objectType == typeof(TimeOnly?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            string timeAsString = (string)reader.Value;
            return TimeOnly.Parse(timeAsString);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                TimeOnly timeOnly = (TimeOnly)value;
                writer.WriteValue(timeOnly.ToString("HH:mm"));
            }
        }
    }

}
