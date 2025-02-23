using System.Reflection;
using System.Text.Json.Serialization;
using DustyEngine.Components;

public class GameObject
{
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public List<GameObject> Children { get; set; } = new List<GameObject>();
    public List<Component> Components { get; set; } = new List<Component>();

    [JsonIgnore] public GameObject Parent { get; set; }


    public void SeActive(bool isActive)
    {
        InvokeMethodInComponents(isActive ? "OnEnable" : "OnDisable");
        Console.WriteLine(Name + " is: " + isActive); // Debug
        IsActive = isActive;
    }

    public void AddComponent(Component component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        Components.Add(component);
        component.Parent = this;
        Console.WriteLine($"Added component: {component.GetType().Name} to {Name}");
    }

    public T? GetComponent<T>() where T : Component
    {
        return Components.OfType<T>().FirstOrDefault();
    }

    public void InvokeMethodInComponents(string methodName)
    {
        //  Console.WriteLine($"{Name} has {Components?.Count ?? 0} components.");
        foreach (Component component in Components ?? Enumerable.Empty<Component>())
        {
            component.Parent = this;
            var startMethod = component.GetType().GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            startMethod?.Invoke(component, null);
            //  Console.WriteLine($"Executed {component.GetType().Name}.{methodName} on {Name}");
        }
    }
}