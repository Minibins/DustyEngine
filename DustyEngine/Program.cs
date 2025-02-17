using System.Text.Json;
using System.Text.Json.Serialization;
using DustyEngine.Components;
using DustyEngine.Json.Converters;
using DustyEngine.Scene;
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




public class TestComponent : Component
{
    public int TestNumber { get; set; }
    public string TestString { get; set; }
}



