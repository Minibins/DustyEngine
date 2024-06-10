using SFML.System;
using SFML.Window;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class CharacterController : Component
{
    float speed;
    public CharacterController(float Speed) 
    { 
        speed = Speed;
    }
    public void FixedUpdate()
    {
        if(Input.IsKeyPressed(Keyboard.Key.D))
        {
            Object.transform.Position = new Vector2f(Object.transform.Position.X += speed,350);
        }
        if(Input.IsKeyPressed(Keyboard.Key.A))
        {
            Object.transform.Position = new Vector2f(Object.transform.Position.X -= speed,350);
        }
    }
}
