using System.Reflection;

namespace DustyEngine.Components;

public class Component
{
    public GameObject Parent { get; set; }
    private bool _isActive;
    public bool IsActive
    {
        get { return _isActive; }
        set
        {
          MethodInfo method = GetType().GetMethod(value ? "OnEnable" : "OnDisable")!;
            Console.WriteLine(GetType().Name + " is: " + value); // Debug
            _isActive = value;
        }
    }
}