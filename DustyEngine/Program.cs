using System.Reflection;
using System.Text.Json;
using DustyEngine.Components;
using DustyEngine.Json.Converters;

namespace DustyEngine
{
    internal class Program
    {
        private static Scene.Scene s_scene;
        public static string ProjectFolderPath { get; set; }

        static void Main(string[] args)
        {
            ProjectFolderPath = "/home/maksym/DustyEngine/DustyEngine/Project";
            s_scene = new Scene.Scene
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
                                    new Transform
                                    {
                                        IsActive = true,
                                        LocalPosition = new Vector3(1f, 0f, 0f),
                                    }
                                }
                            }
                        },
                        Components =
                        {
                            new TestComponent
                            {
                                TestNumber = 20,
                                TestString = "Hello World",
                                IsActive = true,
                            },
                            new Transform
                            {
                                IsActive = true,
                                LocalPosition = new Vector3(0f, 0f, 0f),
                            }
                        }
                    }
                },
            };

            string fileName = s_scene.Name + ".json";
            //
            // string jsonString = JsonSerializer.Serialize(s_scene, new JsonSerializerOptions
            // {
            //     WriteIndented = true,
            //     IncludeFields = true,
            //     Converters =
            //     {
            //         new ComponentConverter(),
            //         new SceneConverter()
            //     }
            // });
            //
            // Console.WriteLine("Serialized JSON:\n" + jsonString);
            //
            // File.WriteAllText(fileName, jsonString);
            //
            // Console.WriteLine("\nRead from file:\n" + File.ReadAllText(fileName));

            Scene.Scene loadedScene = JsonSerializer.Deserialize<Scene.Scene>(
                File.ReadAllText("C:\\Users\\maksym\\Documents\\GitHub\\DustyEngine\\DustyEngine\\DustyEngineTestScene.json"),
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true,
                    Converters =
                    {
                        new ComponentConverter(),
                        new SceneConverter()
                    }
                });


            foreach (var gameObject in loadedScene.GameObjects)
            {
                InvokeOnEnableRecursive(gameObject);
            }

            foreach (var gameObject in loadedScene.GameObjects)
            {
                InvokeStartRecursive(gameObject);
            }

            
            // loadedScene.GameObjects[0].SeActive(false);
            // loadedScene.GameObjects[0].Components[0].SetActive(true);
            // loadedScene.GameObjects[0].SeActive(true);

            // GameObject test = new GameObject
            // {
            //     Name = "TestGameObject3", IsActive = true, Components =
            //     {
            //         new TestComponent
            //         {
            //             IsActive = true
            //         }
            //     }
            // };
            //
            //
            // Transform transform = new Transform
            // {
            //     IsActive = true,
            // };
            //
            // loadedScene.Instantiate(test);
            // test.AddComponent(transform);
            // loadedScene.Destroy(test);
            Task.Run(() => ExecuteFixedUpdateLoop(loadedScene));
            ExecuteUpdateLoop(loadedScene);
        }


        private static void InvokeOnEnableRecursive(GameObject gameObject)
        {
            if (gameObject.IsActive)
            {
                gameObject.InvokeMethodInComponents("OnEnable");
            }

            foreach (var child in gameObject.Children)
            {
                InvokeOnEnableRecursive(child);
            }
        }

        private static void InvokeStartRecursive(GameObject gameObject)
        {
            if (gameObject.IsActive)
            {
                gameObject.InvokeMethodInComponents("Start");
            }

            foreach (var child in gameObject.Children)
            {
                InvokeStartRecursive(child);
            }
        }

        private static void ExecuteUpdateLoop(Scene.Scene scene)
        {
            while (true)
            {
                foreach (var gameObject in scene.GameObjects ?? Enumerable.Empty<GameObject>())
                {
                    if (!gameObject.IsActive) continue;
                    foreach (var component in gameObject.Components ?? Enumerable.Empty<Component>())
                    {
                        var updateMethod = component.GetType().GetMethod("Update",
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        updateMethod?.Invoke(component, null);
                    }
                }
            }
        }

        private static void ExecuteFixedUpdateLoop(Scene.Scene scene)
        {
            var targetElapsedTime = TimeSpan.FromMilliseconds(1);
            var accumulator = TimeSpan.Zero;
            var previousTime = DateTime.Now;

            while (true)
            {
                var currentTime = DateTime.Now;
                var frameTime = currentTime - previousTime;
                previousTime = currentTime;

                accumulator += frameTime;

                while (accumulator >= targetElapsedTime)
                {
                    foreach (var gameObject in scene.GameObjects)
                    {
                        if (gameObject.IsActive)
                        {
                            foreach (var component in gameObject.Components)
                            {
                                var fixedUpdateMethod = component.GetType().GetMethod("FixedUpdate");
                                fixedUpdateMethod?.Invoke(component, null);
                            }
                        }
                    }

                    accumulator -= targetElapsedTime;
                }

                Thread.Sleep(0);
            }
        }
    }
}

public class TestComponent : Component
{
    public int TestNumber { get; set; }
    public string TestString { get; set; }
    private int i = 0;
    private int b = 0;
    private DateTime lastUpdateTime;
    private DateTime lastFixedUpdateTime;

    public void OnEnable()
    {
        lastUpdateTime = DateTime.Now;
        lastFixedUpdateTime = DateTime.Now;

        //Console.WriteLine("Execute OnEnable on: " + Parent.Name + " on " + GetType().Name);
    }
    
    public void OnDisable()
    {
      //  Console.WriteLine("Execute OnDisable on:" + Parent.Name + " " + GetType().Name);
    }

    public void OnDestroy()
    {
      //  Console.WriteLine("Execute OnDestroy on:" + Parent.Name + " " + GetType().Name);
    }

    public void Start()
    {  
       // Console.WriteLine("Execute Start on: " + Parent.Name + " on " + GetType().Name);
        foreach (var parentComponent in Parent.Components)
        {
            Console.WriteLine(parentComponent.GetType().Name);

            // if (parentComponent.GetType().Name == "Player")
            // {
            //     Console.WriteLine("TEst");
            //     parentComponent.SetActive(false);
            // }
        }
         
        Console.WriteLine(Parent.GetComponent<Transform>());
        
        Parent.GetComponent<Player>().SetActive(false);
        Parent.GetComponent<Player>().Parent.SetActive(false);
    }

    public void Update()
    {
        TimeSpan timeSinceLastUpdate = DateTime.Now - lastUpdateTime;

        lastUpdateTime = DateTime.Now;
        i++;
        // Console.WriteLine(
        //S      $"Execute Update on: {Parent.Name} {GetType().Name} {i} (Time since last update: {timeSinceLastUpdate.TotalMilliseconds:F2} ms)");
    }

    public void FixedUpdate()
    {
        TimeSpan timeSinceLastFixedUpdate = DateTime.Now - lastFixedUpdateTime;

        lastFixedUpdateTime = DateTime.Now;
        b++;
        //  Console.WriteLine(
        //    $"Execute FixedUpdate on: {Parent.Name} {GetType().Name} {b} (Time since last fixed update: {timeSinceLastFixedUpdate.TotalMilliseconds:F2} ms)");
    }
}