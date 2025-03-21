using System.Reflection;
using DustyEngine_V3;
using DustyEngine.Components;

namespace DustyEngine;

public class Behaviour: Component
{
    public bool Enabled { get; set; } = true;

    public bool IsActiveAndEnabled => Parent?.IsActive == true && Enabled;
    
    public void SetActive(bool active)
    {
        if (Parent.IsActive)
        {
            MethodInfo method = GetType().GetMethod(active ? "OnEnable" : "OnDisable")!;
            if (method != null)
                method.Invoke(this, null);
            
            Debug.Log($"{GetType().Name} is {(active ? "active" : "inactive")} on GameObject: {Parent.Name}", Debug.LogLevel.Info, true);
            Enabled = active;
        }
    }
}