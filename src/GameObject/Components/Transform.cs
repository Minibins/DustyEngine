using SFML.System;
public class Transform
{
    public Vector3f Position;
    public Vector3f Scale;
    public Vector3f Rotation;
    public Transform(Vector3f position,Vector3f scale,Vector3f rotation)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;
    }
}