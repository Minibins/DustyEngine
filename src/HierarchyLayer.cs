using System.Collections.Generic;
using System.Reflection;

public abstract class HierarchyLayer
{
    protected List<object> children = new List<object>();
}

public abstract class MessagesRestreamer : HierarchyLayer
{
    public void RestreamingMethod(string name)
    {
        foreach (var child in children)
        {
            MethodInfo[] methods = child.GetType().GetMethods();
            foreach (var method in methods)
            {
                if (method.Name == name) method.Invoke(child, null);
                if (method.Name == nameof(RestreamingMethod)) method.Invoke(child, new object[] {name});
            }
        }
    }
}