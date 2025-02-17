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
                    IsActive = true,
                    Children =
                    {
                        new GameObject
                        {
                            Name = "TestGameObject2",
                            IsActive = true,
                            Components =
                            {
                                new TestComponent
                                {
                                    TestNumber = 20,
                                    TestString = "Hello World"
                                }
                            }
                        }
                    },
                    Components =
                    {
                        new TestComponent
                        {
                            TestNumber = 20,
                            TestString = "Hello World"
                        }
                    }
                },
                new GameObject
                {
                    Name = "TestGameObject1",
                    IsActive = true,
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
        
        
     
        void InvokeStartRecursive(GameObject gameObject)
        {
            if (gameObject.IsActive)
            {
               gameObject.InvokeMethodInComponents("OnEnable");
               gameObject.InvokeMethodInComponents("Start");
            }
            foreach (var child in gameObject.Children)
            {
                InvokeStartRecursive(child);
            }
        }
        
        foreach (var gameObject in loadedScene.GameObjects)
        {
            InvokeStartRecursive(gameObject);
        }
        loadedScene.GameObjects[0].IsActive = false;
        loadedScene.GameObjects[0].IsActive = true;
    }
}


public class TestComponent : Component
{
    public int TestNumber { get; set; }
    public string TestString { get; set; }

    public void OnEnable()
    {
        Console.WriteLine(TestNumber);
    }

    public void OnDisable()
    {
        Console.WriteLine("Disable");
    }

    public void Start()
    {
        Console.WriteLine(TestString);
    }
}