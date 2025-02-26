using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DustyEngine.Components;

namespace DustyEngine.Json.Converters
{
    public class ComponentConverter : JsonConverter<Component>
    {
        private static readonly Dictionary<string, Type> ComponentTypes;

        static ComponentConverter()
        {
            // Получаем ВСЕ загруженные сборки
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Ищем все классы, унаследованные от Component
            ComponentTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Component)))
                .ToDictionary(t => t.Name, t => t);
        }

        public override Component Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                string typeName = doc.RootElement.GetProperty("Type").GetString();

                if (!ComponentTypes.TryGetValue(typeName, out Type componentType))
                    throw new JsonException($"Unknown component: {typeName}");

                var newOptions = new JsonSerializerOptions(options) { Converters = { this } };
                return (Component)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), componentType, newOptions)!;
            }
        }

        public override void Write(Utf8JsonWriter writer, Component value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Type", value.GetType().Name);

            var type = value.GetType();

            foreach (var member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (member.Name.Contains("k__BackingField")) continue; // Игнорируем бекерные поля

                object? memberValue = member switch
                {
                    PropertyInfo prop when prop.CanRead => prop.GetValue(value),
                    FieldInfo field => field.GetValue(value),
                    _ => null
                };

                if (memberValue != null)
                {
                    writer.WritePropertyName(member.Name);
                    JsonSerializer.Serialize(writer, memberValue, options);
                }
            }

            writer.WriteEndObject();
        }
    }
}
