using System.Reflection;

namespace DustyEngine.Components;

public class Component
{
    public GameObject Parent { get; set; }
    public bool IsActive { get; set; }

    public void SetActive(bool active)
    {
        if (Parent.IsActive)
        {
            MethodInfo method = GetType().GetMethod(active ? "OnEnable" : "OnDisable")!;
            if (method != null)
                method.Invoke(this, null);

            Console.WriteLine(GetType().Name + " is: " + active + " on: " + Parent.Name);
            IsActive = active;
        }
    }
}