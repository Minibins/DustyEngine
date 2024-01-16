using System;
using DustyEngine;
using DustyEngine.GameObject;
using DustyEngine.Input;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Window = DustyEngine.Window.Window;

public class Game
{
    public void Start()
    {
        Scene scene = new Scene();

        Window  window = new Window(640, 480, 60, "test", scene);
        
        GameObject obj = new GameObject();
        
         scene.Instantiate(obj);
         
         obj.color = Color.Green;
         
         obj.transform.Position = new Vector2f(50,350);
         obj.transform.Scale = new Vector2f(50,50);
         
         obj.UpdateObj();
         
         
         GameObject obj1 = new GameObject();
        
         scene.Instantiate(obj1);
         
         obj1.color = Color.White;
         
         obj1.transform.Position = new Vector2f(20,400);
         obj1.transform.Scale = new Vector2f(600,50);
         
         obj1.UpdateObj();
         
    }


    public void Update()    
    {

        if (Input.IsKeyPressed(Keyboard.Key.D))
        {
           Console.WriteLine("Pressed");
        }
    }
}