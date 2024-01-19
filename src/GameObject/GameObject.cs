using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

public class GameObject : MessagesRestreamer
{
    public RectangleShape Shape { get; set; }
    public Vector2f Velocity { get; set; }

    public Transform Transform = new Transform(new Vector2f(100, 100), new Vector2f(50, 50));

    public Color Color = Color.Blue;
    public List<Component> Components
    {
        get
        {
            return _children.OfType<Component>().ToList();
        }
    }
    public GameObject()
    {
        Shape = new RectangleShape(Transform.Scale)
        {
            Position = Transform.Position,
            FillColor = Color
        };
        Velocity = new Vector2f(0, 0);
    }

    public void Update()
    {
        Shape.Position = Transform.Position;
        Shape.Size = Transform.Scale;
        Shape.FillColor = Color;
    }
    
    public void Draw(RenderWindow window)
    {
        window.Draw(Shape);
    }
    public void AddComponent(Component component)
    {
        _children.Add(component);
        component.GameObject = this;
    }
}