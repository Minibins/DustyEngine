using SFML.System;
using SFML.Window;

public class CharacterController : Component
{
   private float _speed;
    public CharacterController(float _speed) 
    { 
        this._speed = _speed;
    }
    public void FixedUpdate()
    {
        if(Input.IsKeyPressed(Keyboard.Key.D))
        {
            GameObject.Transform.Position = new Vector2f(GameObject.Transform.Position.X += _speed,350);
        }
        if(Input.IsKeyPressed(Keyboard.Key.A))
        {
            GameObject.Transform.Position = new Vector2f(GameObject.Transform.Position.X -= _speed,350);
        }
    }
}
