using SFML.Graphics;
using SFML.System;

public class GameObject : MessagesRestreamer
{
    public RectangleShape Shape { get; set; }
    public Vector2f Velocity { get; set; }

    public Transform transform = new Transform(new Vector2f(100, 100), new Vector2f(50, 50));

    public Color color = Color.Blue;
    public List<Component> Components
    {
        get
        {
            return children.OfType<Component>().ToList();
        }
    }
    public GameObject()
    {
        Shape = new RectangleShape(transform.Scale)
        {
            Position = transform.Position,
            FillColor = color
        };
        Velocity = new Vector2f(0, 0);
    }

    public void Update()
    {
        Shape.Position = transform.Position;
        Shape.Size = transform.Scale;
        Shape.FillColor = color;
    }
    
    public void Draw(RenderWindow window)
    {
        window.Draw(Shape);
    }
    public void AddComponent(Component component)
    {
        children.Add(component);
        component.GameObject = this;
    }
}