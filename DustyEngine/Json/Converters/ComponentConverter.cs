﻿using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DustyEngine_V3;
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
                string typeName = doc.RootElement.GetProperty("Type").GetString();

                if (!ComponentTypes.TryGetValue(typeName, out Type componentType))
                    throw new JsonException($"Unknown component: {typeName}");

                if (doc.RootElement.TryGetProperty("sourcePath", out JsonElement externalSourcePath))
                {
                    string sourcePath = externalSourcePath.GetString();
                    Debug.Log($"Source Path: {sourcePath}", Debug.LogLevel.Info, true);

                    Component? externalComponent = LoadOrCompileComponent(sourcePath);
                    if (externalComponent != null)
                    {
                        componentType = externalComponent.GetType();
                    }
                }

                var newOptions = new JsonSerializerOptions(options)
                {
                    Converters = { this }
                };

                return (Component)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), componentType, newOptions)!;
            }
        }


        public static Component? LoadOrCompileComponent(string path)
        {
            string typeName = Path.GetFileNameWithoutExtension(path);
            if (Path.GetExtension(path).Equals(".dll", StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"Loading component from DLL: {path}", Debug.LogLevel.Info, true);
                return LoadComponentFromDll(path, typeName);
            }
            else if (Path.GetExtension(path).Equals(".cs", StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"Compiling component from source: {path}", Debug.LogLevel.Info, true);
                string dllPath = CompileSourceToDll(path);
                return LoadComponentFromDll(dllPath, typeName);
            }
            else
            {
                Debug.Log($"Unsupported file type: {path}", Debug.LogLevel.Error, true);
            }

            return null;
        }


        private static Component? LoadComponentFromDll(string dllPath, string typeName)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                var type = assembly.GetType(typeName);
                if (type == null)
                {
                    Debug.Log($"Type '{typeName}' not found in '{dllPath}'", Debug.LogLevel.Error);
                    return null;
                }

                Debug.Log($"Compiling source: {dllPath}, detected typeName: {typeName}", Debug.LogLevel.Info, true);

                Debug.Log($"Loading component from assembly: {assembly.FullName}", Debug.LogLevel.Info, true);

                return (Component)Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                Debug.Log($"Error loading component from DLL: {ex.Message}", Debug.LogLevel.Error, true);
                throw;
            }
        }


        private static string CompileSourceToDll(string sourcePath)
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
                    Debug.Log($"Using existing DLL: {outputDllPath}", Debug.LogLevel.Info, true);
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
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
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
                        Debug.Log($"Compilation error: {diagnostic.GetMessage()}", Debug.LogLevel.Error);
                    }

                    throw new Exception();
                }
            }

            Debug.Log($"Compiled new DLL at: {outputDllPath}", Debug.LogLevel.Info, true);
            return outputDllPath;
        }


public override void Write(Utf8JsonWriter writer, Component value, JsonSerializerOptions options)
{
    writer.WriteStartObject();
    writer.WriteString("Type", value.GetType().Name);

    var type = value.GetType();
    var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    foreach (var member in members)
    {
        if (member.Name.Contains("k__BackingField"))
            continue;

        if (HasJsonIgnore(member, type))
            continue;

        bool shouldSerialize = false;

        if (member is FieldInfo field)
        {
            shouldSerialize = field.IsPublic || field.GetCustomAttribute<SerializeFieldAttribute>() != null;
        }
        else if (member is PropertyInfo prop)
        {
            // Публичное свойство с get
            if (prop.CanRead && prop.GetMethod?.IsPublic == true)
                shouldSerialize = true;

            // Или приватное с [SerializeField]
            if (prop.GetCustomAttribute<SerializeFieldAttribute>() != null && prop.CanRead)
                shouldSerialize = true;
        }

        if (!shouldSerialize)
            continue;

        object? valueToWrite = null;

        try
        {
            valueToWrite = member switch
            {
                FieldInfo f => f.GetValue(value),
                PropertyInfo p => p.GetValue(value),
                _ => null
            };
        }
        catch (Exception ex)
        {
            Debug.Log($"[Warning] Skipping '{member.Name}': {ex.Message}", Debug.LogLevel.Warning);
            continue;
        }

        if (valueToWrite != null)
        {
            try
            {
                writer.WritePropertyName(member.Name);
                JsonSerializer.Serialize(writer, valueToWrite, options);
            }
            catch (Exception ex)
            {
                Debug.Log($"[Warning] Failed to serialize '{member.Name}': {ex.Message}", Debug.LogLevel.Warning);
            }
        }
    }

    writer.WriteEndObject();
}
private static bool HasJsonIgnore(MemberInfo member, Type declaringType)
{
    // Прямо на члене
    if (member.GetCustomAttribute<JsonIgnoreAttribute>() != null)
        return true;

    // Если это свойство — ищем оригинал в типе
    if (member is PropertyInfo prop)
    {
        var realProp = declaringType.GetProperty(prop.Name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (realProp?.GetCustomAttribute<JsonIgnoreAttribute>() != null)
            return true;
    }

    // То же самое для полей
    if (member is FieldInfo field)
    {
        var realField = declaringType.GetField(field.Name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (realField?.GetCustomAttribute<JsonIgnoreAttribute>() != null)
            return true;
    }

    return false;
}


    }
}