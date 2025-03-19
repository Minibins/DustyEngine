using System.Reflection;
using System.Text.Json;
using DustyEngine_V3;
using DustyEngine.Components;
using DustyEngine.Json.Converters;

namespace DustyEngine
{
    internal static class Program
    {
        private static Scene.Scene s_scene;
        public static string ProjectFolderPath { get; set; }
        public static ProjectSettings settings = new ProjectSettings();

        static void Main(string[] args)
        {
            Debug.ClearLogs();
            
            ProjectFolderPath = "C:\\Users\\maksym\\Documents\\GitHub\\DustyEngine\\DustyEngine\\Project";
            
            ProjectSettings projectSettings = new ProjectSettings
            {
                ProjectName = "My Game",
                Version = 1.0f,
                PathToScenes = new List<String>(),
                Debug = false,
                LogLevel = Debug.LogLevel.Info,
                LogToConsole = true,
                LogToFile = true,
            };

            SerializeProjectSettings(projectSettings);
            
            Debug.Log("Starting Dusty Engine", Debug.LogLevel.Info, false);

            if (ProjectFolderPath != null)
                Debug.Log("Project folder path: " + ProjectFolderPath, Debug.LogLevel.Info, true);
            else
                Debug.Log("Project folder path is null", Debug.LogLevel.FatalError, false);

            DeserializeProjectSettings();

            Debug.EnableDebugMode(settings.Debug);
            Debug.SetLogLevel(settings.LogLevel);
            Debug.EnableConsoleLogging(settings.LogToConsole);
            Debug.EnableFileLogging(settings.LogToFile);

            Debug.Log("Project settings loaded", Debug.LogLevel.Info, false);

            Debug.Log($"Initial currentLogLevel:  {Debug.GetLogLevel()}", Debug.LogLevel.Info, true);
            Debug.Log("Test INFO", Debug.LogLevel.Info, true);
            Debug.Log("Test WARNING", Debug.LogLevel.Warning, true);
            Debug.Log("Test ERROR", Debug.LogLevel.Error, true);
            Debug.Log("Test FATAL", Debug.LogLevel.FatalError, true);
            
            if (LoadScene(out var loadedScene)) return;

            foreach (var method in new[] { "OnEnable", "Start" })
            {
                foreach (var gameObject in loadedScene.GameObjects)
                {
                    InvokeRecursive(gameObject, method);
                }
            }

            TestScene(loadedScene);


            Task.Run(() => ExecuteFixedUpdateLoop(loadedScene));
            ExecuteUpdateLoop(loadedScene);
        }

        private static void TestScene(Scene.Scene? loadedScene)
        {
            // loadedScene.GameObjects[0].SeActive(false);
            // loadedScene.GameObjects[0].Components[0].SetActive(true);
            // loadedScene.GameObjects[0].SeActive(true);

            GameObject test = new GameObject
            {
                Name = "TestGameObject3", IsActive = true, Components =
                {
                    new TestComponent
                    {
                        IsActive = true
                    }
                }
            };


            Transform transform = new Transform
            {
                IsActive = true,
            };

            loadedScene.Instantiate(test, loadedScene.GameObjects[0]);
            // test.AddComponent(transform);
            // loadedScene.Destroy(test);
            //    Debug.ShowLogs();
        }

        private static void DeserializeProjectSettings()
        {
            string filePath = Path.Combine(ProjectFolderPath, "project_settings.json");

            if (!File.Exists(filePath))
            {
                Debug.Log("Project settings file not found", Debug.LogLevel.FatalError, true);
            }
            else
            {
                string fileContent = File.ReadAllText(filePath);
                settings = JsonSerializer.Deserialize<ProjectSettings>(fileContent);

                if (settings == null)
                {
                    Debug.Log("Project settings could not be loaded", Debug.LogLevel.FatalError, true);
                }
            }
        }
        private static void SerializeProjectSettings(ProjectSettings projectSettings)
        {
            string json = JsonSerializer.Serialize(projectSettings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(ProjectFolderPath, "project_settings.json"), json);
        }

        private static bool LoadScene(out Scene.Scene? loadedScene)
        {
            loadedScene = new Scene.Scene();
            try
            {
                string scenePath =
                    "C:\\Users\\maksym\\Documents\\GitHub\\DustyEngine\\DustyEngine\\DustyEngineTestScene.json";

                Debug.Log($"Starting scene loading from: {scenePath}", Debug.LogLevel.Info, true);

                if (!File.Exists(scenePath))
                {
                    Debug.Log($"Scene file not found: {scenePath}", Debug.LogLevel.FatalError, false);
                    return true;
                }

                loadedScene = JsonSerializer.Deserialize<Scene.Scene>(
                    File.ReadAllText(scenePath),
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

                Debug.Log("Scene successfully loaded!", Debug.LogLevel.Info, false);
            }
            catch (Exception ex)
            {
                Debug.Log($"Error loading scene: {ex.Message}", Debug.LogLevel.FatalError, false);
            }

            return false;
        }
        
        private static void InvokeRecursive(GameObject gameObject, string methodName)
        {
            if (gameObject.IsActive)
            {
                gameObject.InvokeMethodInComponents(methodName);
            }

            foreach (var child in gameObject.Children)
            {
                InvokeRecursive(child, methodName);
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

public class ProjectSettings
{
    public string ProjectName { get; set; }
    public float Version { get; set; }
    public List<string> PathToScenes { get; set; }
    public bool Debug { get; set; }
    public Debug.LogLevel LogLevel { get; set; }
    public bool LogToConsole { get; set; }
    public bool LogToFile { get; set; }
}
