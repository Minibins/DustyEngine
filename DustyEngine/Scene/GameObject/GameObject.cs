using System.Reflection;
using System.Text.Json.Serialization;
using DustyEngine.Components;

public class GameObject
{
    public string Name { get; set; }
    private bool _isActive;

    public bool IsActive
    {
        get { return _isActive; }
        set
        {
            InvokeMethodInComponents(value ? "OnEnable" : "OnDisable");
            Console.WriteLine(Name + " is: " + value); // Debug
            _isActive = value;
        }
    }

    public List<GameObject> Children { get; set; } = new List<GameObject>();
    public List<Component> Components { get; set; } = new List<Component>();

    [JsonIgnore] public GameObject Parent { get; set; }


    public void InitComponents()
    {
        foreach (var component in Components)
        {
            component.Parent = this;
        }
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
            var startMethod = component.GetType().GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            startMethod?.Invoke(component, null);
            //  Console.WriteLine($"Executed {component.GetType().Name}.{methodName} on {Name}");
        }
    }
}