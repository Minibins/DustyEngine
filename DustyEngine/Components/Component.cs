namespace DustyEngine.Components;

public class Component : Object
{
    public GameObject Parent { get; set; }
    
    public T? GetComponent<T>() where T : Component
    {
        return Parent?.GetComponent<T>();
    }
}