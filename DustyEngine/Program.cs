using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using DustyEngine.Components;
using DustyEngine.Json.Converters;

namespace DustyEngine
{
    internal class Program
    {
        private static Scene.Scene s_scene;

        static void Main(string[] args)
        {
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
                            }
                        }
                    },
                    new GameObject
                    {
                        Name = "TestGameObject1",
                        IsActive = true,
                    },
                },
            };

            string fileName = s_scene.Name + ".json";

            string jsonString = JsonSerializer.Serialize(s_scene, new JsonSerializerOptions
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

            Scene.Scene loadedScene = JsonSerializer.Deserialize<Scene.Scene>(jsonString, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                Converters =
                {
                    new ComponentConverter(),
                    new SceneConverter()
                }
            });


            foreach (var gameObject in s_scene.GameObjects)
            {
                InvokeStartRecursive(gameObject);
            }


            loadedScene.GameObjects[0].Components[0].SetActive(false);
            loadedScene.GameObjects[0].SeActive(false);
            loadedScene.GameObjects[0].Components[0].SetActive(true);
            loadedScene.GameObjects[0].SeActive(true);

            Task.Run(() => ExecuteFixedUpdateLoop(s_scene));
            ExecuteUpdateLoop(s_scene);
        }

        
        private static void InvokeStartRecursive(GameObject gameObject)
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
        // if (Parent == null)
        // {
        //     Console.WriteLine("Parent is still null in OnEnable.");
        // }
        // else
        // {
        //     Console.WriteLine("Parent is set: " + Parent.Name);
        // }
        Console.WriteLine("Execute OnEnable on: " + Parent.Name + " on " + GetType().Name);
        //  Parent.GetComponent<TestComponent>()?.TestMethod();
    }

    public void TestMethod()
    {
        Console.WriteLine("Execute TestMethod on:" + Parent.Name + " " + GetType().Name);
    }

    public void OnDisable()
    {
        Console.WriteLine("Execute OnDisable on:" + Parent.Name + " " + GetType().Name);
    }

    public void Start()
    {
        Console.WriteLine("Execute Start on:" + Parent.Name + " " + GetType().Name);
    }

    public void Update()
    {
        TimeSpan timeSinceLastUpdate = DateTime.Now - lastUpdateTime;

        // Обновляем время последнего запуска
        lastUpdateTime = DateTime.Now;
        i++;
        // Console.WriteLine(
        //S      $"Execute Update on: {Parent.Name} {GetType().Name} {i} (Time since last update: {timeSinceLastUpdate.TotalMilliseconds:F2} ms)");
    }

    public void FixedUpdate()
    {
        TimeSpan timeSinceLastFixedUpdate = DateTime.Now - lastFixedUpdateTime;

        // Обновляем время последнего вызова FixedUpdate
        lastFixedUpdateTime = DateTime.Now;
        b++;
        //  Console.WriteLine(
        //    $"Execute FixedUpdate on: {Parent.Name} {GetType().Name} {b} (Time since last fixed update: {timeSinceLastFixedUpdate.TotalMilliseconds:F2} ms)");
    }
}