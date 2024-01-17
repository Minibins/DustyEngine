using SFML.System;
    public class Transform
    {
        public Vector2f Position;
        public Vector2f Scale;

        public Transform(Vector2f position, Vector2f scale)
        {
            Position = position;
            Scale = scale;
        }
    }