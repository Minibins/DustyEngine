using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

internal class Program
{
    private static Scene scene;

    static void Main(string[] args)
    {
        scene = new Scene
        {
            Name = "DustyEngineTestScene",
            GameObjects = new List<GameObject>
            {
                new GameObject
                {
                    Name = "TestGameObject0",
                    Children =
                    {
                        new GameObject
                        {
                            Name = "TestGameObject2",
                            Components = {new TestComponent
                            {
                                TestNumber = 20,
                                TestString = "Hello World"
                            }}
                        }
                    },
                    Components = {new TestComponent
                    {
                        TestNumber = 20,
                        TestString = "Hello World"
                    }}
                },
                new GameObject
                {
                    Name = "TestGameObject1",
                },
            },
            Components = new List<Component>
            {
                new TestComponent()
                {
                    TestNumber = 0,
                    TestString = "TestString"
                }
            },
        };

        string fileName = scene.Name + ".json";
        
        string jsonString = JsonSerializer.Serialize(scene, new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
            Converters =
            {
                new ComponentConverter(),
                new SceneConverter()
            }
        });

        Console.WriteLine("Serialized JSON:\n" + jsonString);

        File.WriteAllText(fileName, jsonString);

        Console.WriteLine("\nRead from file:\n" + File.ReadAllText(fileName));
        
        Scene loadedScene = JsonSerializer.Deserialize<Scene>(jsonString, new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
            Converters =
            {
                new ComponentConverter(),
                new SceneConverter()
            }
        });

        Console.WriteLine(loadedScene.GameObjects[0].Children[0].Parent.Name);
    }
}

public class GameObject
{
    public string Name { get; set; }
    public List<GameObject> Children { get; set; } = new List<GameObject>();
    public List<Component> Components { get; set; } = new List<Component>();

    [JsonIgnore] public GameObject Parent { get; set; }
}

public class Scene
{
    public string Name { get; set; }
    public List<GameObject> GameObjects { get; set; } = new List<GameObject>();
    public List<Component> Components { get; set; } = new List<Component>();
}

public class Component
{
}

public class TestComponent : Component
{
    public int TestNumber { get; set; }
    public string TestString { get; set; }
}

public class SceneConverter : JsonConverter<Scene>
{
    public override Scene Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var scene = new Scene();

            if (doc.RootElement.TryGetProperty("Name", out var nameElement))
            {
                scene.Name = nameElement.GetString();
            }

            if (doc.RootElement.TryGetProperty("GameObjects", out var gameObjectsElement))
            {
                scene.GameObjects = DeserializeGameObjects(gameObjectsElement, null);
            }

            if (doc.RootElement.TryGetProperty("Components", out var componentsElement))
            {
                scene.Components = JsonSerializer.Deserialize<List<Component>>(componentsElement.GetRawText(), options);
            }

            return scene;
        }
    }

    private List<GameObject> DeserializeGameObjects(JsonElement element, GameObject parent)
    {
        var gameObjects = new List<GameObject>();

        foreach (var objElement in element.EnumerateArray())
        {
            var gameObject = new GameObject();

            if (objElement.TryGetProperty("Name", out var nameElement))
            {
                gameObject.Name = nameElement.GetString();
            }

            gameObject.Parent = parent;

            if (objElement.TryGetProperty("Children", out var childrenElement))
            {
                gameObject.Children = DeserializeGameObjects(childrenElement, gameObject);
            }

            gameObjects.Add(gameObject);
        }

        return gameObjects;
    }

    public override void Write(Utf8JsonWriter writer, Scene value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("Name", value.Name);

        writer.WritePropertyName("GameObjects");
        JsonSerializer.Serialize(writer, value.GameObjects, options);

        writer.WritePropertyName("Components");
        JsonSerializer.Serialize(writer, value.Components, options);

        writer.WriteEndObject();
    }
}

public class ComponentConverter : JsonConverter<Component>
{
    public override Component Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            string type = doc.RootElement.GetProperty("Type").GetString();
            
            // Создаем новые опции без этого конвертера, чтобы избежать рекурсии
            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.Clear();
            
            Type componentType;
            switch (type)
            {
                case "TestComponent":
                    componentType = typeof(TestComponent);
                    break;
                // Добавьте другие типы компонентов здесь
                default:
                    throw new JsonException($"Неизвестный компонент: {type}");
            }

            // Десериализуем объект напрямую в нужный тип
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