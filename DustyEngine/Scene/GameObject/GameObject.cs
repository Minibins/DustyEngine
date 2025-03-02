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
        Console.WriteLine(Name + " is: " + isActive);
        IsActive = isActive;
    }

    public void AddComponent(Component component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        Components.Add(component);
        component.Parent = this;
        Console.WriteLine($"Added component: {component.GetType().Name} to {Name}");
    }

    public void Destroy()
    {
        InvokeMethodInComponents("OnDestroy");

        Components.Clear();
    }

    public T? GetComponent<T>() where T : Component
    {
        return Components.OfType<T>().FirstOrDefault();
    }

    public void InvokeMethodInComponents(string methodName)
    {
        Console.WriteLine($"[{Name}] has {Components?.Count ?? 0} components.");

        foreach (Component component in Components ?? Enumerable.Empty<Component>())
        {
            component.Parent = this;

            try
            {
                var method = component.GetType().GetMethod(methodName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (method != null)
                {
                    Console.WriteLine($"Executing {component.GetType().Name}.{methodName} on {Name}");
                    method.Invoke(component, null);
                }
                else
                {
                    Console.WriteLine($"Method {methodName} not found in {component.GetType().Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing {methodName} in {component.GetType().Name}: {ex.Message}");
            }
        }
    }
}