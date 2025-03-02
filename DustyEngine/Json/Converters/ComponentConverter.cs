using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DustyEngine.Components;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DustyEngine.Json.Converters
{
    public class ComponentConverter : JsonConverter<Component>
    {
        private static readonly Dictionary<string, Type> ComponentTypes;

        static ComponentConverter()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            ComponentTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Component)))
                .ToDictionary(t => t.Name, t => t);
        }

        public override Component Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                // Console.WriteLine($"Raw JSON: {doc.RootElement.GetRawText()}");

                string typeName = doc.RootElement.GetProperty("Type").GetString();

                if (!ComponentTypes.TryGetValue(typeName, out Type componentType))
                    throw new JsonException($"Unknown component: {typeName}");

                if (doc.RootElement.TryGetProperty("sourcePath", out JsonElement externalSourcePath))
                {
                    string sourcePath = externalSourcePath.GetString();
                    Console.WriteLine($"Source Path: {sourcePath}");
                    Component? externalComponent = LoadOrCompileComponent(sourcePath);
                    Console.WriteLine($"Component Type: {externalComponent.GetType().Name}");
                }

                var newOptions = new JsonSerializerOptions(options) { Converters = { this } };
                return (Component)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), componentType, newOptions)!;
            }
        }


        private Component? LoadOrCompileComponent(string path)
        {
            string typeName = Path.GetFileNameWithoutExtension(path);
            if (Path.GetExtension(path).Equals(".dll", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Loading component from DLL: {path}");
                return LoadComponentFromDll(path, typeName);
            }
            else if (Path.GetExtension(path).Equals(".cs", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Compiling component from source: {path}");
                string dllPath = CompileSourceToDll(path);
                return LoadComponentFromDll(dllPath, typeName);
            }
            else
            {
                throw new NotSupportedException($"Unsupported file type: {path}");
            }

            return null;
        }


        private Component? LoadComponentFromDll(string dllPath, string typeName)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                var type = assembly.GetType(typeName)
                           ?? throw new InvalidOperationException($"Type '{typeName}' not found in '{dllPath}'");
                Console.WriteLine($"Compiling source: {dllPath}, detected typeName: {typeName}");

                Console.WriteLine($"Loading component from assembly: {assembly.FullName}");

                return (Component)Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading component from DLL: {ex.Message}");
                throw;
            }
        }


        private string CompileSourceToDll(string sourcePath)
        {
            string outputDirectory = Path.Combine(Program.ProjectFolderPath, "Dlls");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string outputDllPath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(sourcePath) + ".dll");

            if (File.Exists(outputDllPath))
            {
                DateTime sourceLastModified = File.GetLastWriteTime(sourcePath);
                DateTime dllLastModified = File.GetLastWriteTime(outputDllPath);

                if (sourceLastModified <= dllLastModified)
                {
                    Console.WriteLine($"Using existing DLL: {outputDllPath}");
                    return outputDllPath;
                }
            }

            string sourceCode = File.ReadAllText(sourcePath);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var root = syntaxTree.GetRoot();
            
            var usingDirectives = root.DescendantNodes()
                .OfType<UsingDirectiveSyntax>()
                .Select(u => u.Name.ToString())
                .Distinct()
                .ToList();
            
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Private.CoreLib
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Console").Location),
                MetadataReference.CreateFromFile(Assembly.Load("Microsoft.CSharp").Location),
                MetadataReference.CreateFromFile(Assembly.GetExecutingAssembly().Location) // DustyEngine.dll
            };

            
            foreach (var ns in usingDirectives)
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetTypes().Any(t => t.Namespace == ns));

                if (assembly != null && !references.Any(r => r.Display == assembly.Location))
                {
                    references.Add(MetadataReference.CreateFromFile(assembly.Location));
                }
            }
            
            var compilation = CSharpCompilation.Create(
                Path.GetFileNameWithoutExtension(outputDllPath),
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using (var fileStream = new FileStream(outputDllPath, FileMode.Create))
            {
                var result = compilation.Emit(fileStream);

                if (!result.Success)
                {
                    foreach (var diagnostic in result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
                    {
                        Console.WriteLine($"Compilation error: {diagnostic.GetMessage()}");
                    }

                    throw new Exception("Compilation failed.");
                }
            }

            Console.WriteLine($"Compiled new DLL at: {outputDllPath}");
            return outputDllPath;
        }


        public override void Write(Utf8JsonWriter writer, Component value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Type", value.GetType().Name);

            var type = value.GetType();

            foreach (var member in
                     type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (member.Name.Contains("k__BackingField")) continue;

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