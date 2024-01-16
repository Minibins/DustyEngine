using SFML.Graphics;
using SFML.System;

namespace DustyEngine.GameObject
{
    public class GameObject
    {
        public RectangleShape Shape { get; set; }
        public Vector2f Velocity { get; set; }
        

        public GameObject(Vector2f position, Vector2f size, Color color)
        {
            Shape = new RectangleShape(size)
            {
                Position = position,
                FillColor = color
            };
            Velocity = new Vector2f(0, 0);
        }
        
        public void Draw(RenderWindow window)
        {
            window.Draw(Shape);
        }
    }
}