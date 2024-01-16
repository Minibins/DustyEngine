using System;
using SFML.Graphics;
using SFML.System;

namespace DustyEngine.GameObject
{
    public class GameObject
    {
        public RectangleShape Shape { get; set; }
        public Vector2f Velocity { get; set; }

        public Components.Transform transform = new Components.Transform(new Vector2f(100,100), new Vector2f(50,50));

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

        public void UpdateObj()
        {
            Shape.Position = transform.Position;
            Shape.Size = transform.Scale;
            Shape.FillColor = color;
            
            Console.WriteLine("Object:");
            Console.WriteLine(transform.Position);
            Console.WriteLine(transform.Scale);
            Console.WriteLine(color + "\n");
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(Shape);
        }
    }
}