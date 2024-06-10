using SFML.System;

public class Object: MessagesRestreamer
{
    public List<Component> Components
    {
        get
        {
            return children.OfType<Component>().ToList();
        }
    }

    public void AddComponent(Component component)
    {
        children.Add(component);
        component.Object = this;
    }
}
