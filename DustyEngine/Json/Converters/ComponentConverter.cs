using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DustyEngine.Components;

namespace DustyEngine.Json.Converters;

public class ComponentConverter : JsonConverter<Component>
{
    public override Component Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            string type = doc.RootElement.GetProperty("Type").GetString();
            
            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.Clear();
            
            Type componentType;
            switch (type)
            {
                case "TestComponent":
                    componentType = typeof(TestComponent);
                    break;
                default:
                    throw new JsonException($"Unknown component: {type}");
            }

            return (Component)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), componentType, newOptions);
        }
    }

    public override void Write(Utf8JsonWriter writer, Component value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Type", value.GetType().Name);

        foreach (PropertyInfo property in value.GetType().GetProperties())
        {
            object propValue = property.GetValue(value);
            if (propValue != null)
            {
                writer.WritePropertyName(property.Name);
                JsonSerializer.Serialize(writer, propValue, options);
            }
        }

        foreach (FieldInfo field in value.GetType().GetFields())
        {
            object fieldValue = field.GetValue(value);
            if (fieldValue != null)
            {
                writer.WritePropertyName(field.Name);
                JsonSerializer.Serialize(writer, fieldValue, options);
            }
        }

        writer.WriteEndObject();
    }
}