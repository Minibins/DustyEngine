using System.Reflection;
using DustyEngine_V3;

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
            
            Debug.Log($"{GetType().Name} is {(active ? "active" : "inactive")} on GameObject: {Parent.Name}", Debug.LogLevel.Info, true);
            IsActive = active;
        }
    }
}