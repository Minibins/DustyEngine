using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Game  
{
    private GameObject obj;
    
    public void Start()
    {
        obj = new GameObject();
        
        Scene.Instantiate(obj);

        obj.color = Color.Green;

        obj.transform.Position = new Vector2f(50, 350);
        obj.transform.Scale = new Vector2f(50, 50);
        

        GameObject obj1 = new GameObject();

        Scene.Instantiate(obj1);

        obj1.color = Color.White;

        obj1.transform.Position = new Vector2f(20, 400);
        obj1.transform.Scale = new Vector2f(600, 50);
        
    }


    public void Update()
    {
        if (Input.IsKeyPressed(Keyboard.Key.D))
        {
            Console.WriteLine("Pressed: D");
            obj.transform.Position = new Vector2f(obj.transform.Position.X += 0.1f, 350);
        }
        if (Input.IsKeyPressed(Keyboard.Key.A))
        {
            Console.WriteLine("Pressed: A");
            obj.transform.Position = new Vector2f(obj.transform.Position.X -= 0.1f, 350);
        }
    }
}