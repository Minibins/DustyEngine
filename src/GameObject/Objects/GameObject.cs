using SFML.Graphics;
using SFML.System;

public class GameObject : Object
{
    public RectangleShape Shape { get; set; }
    public Vector2f Velocity { get; set; }
    public Transform transform = new Transform(new(100, 100, 0), new(50, 50, 0), new());

    public Color color = Color.Blue;
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
}