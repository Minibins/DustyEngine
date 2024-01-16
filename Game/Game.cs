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
    private Scene scene;
    private Window window;
    private GameObject obj;

    public void Start()
    {
        scene = new Scene();

        window = new Window(640, 480, 60, "test", scene);
        
        
         obj = new GameObject();
       
         
         scene.Instantiate(obj);
         
         obj.color = Color.Green;
         
         obj.transform.Scale = new Vector2f(20,20);
         
         obj.UpdateObj();
    }


    public void Update()    
    {
        //    Console.WriteLine(obj.Position);
        if (Input.IsKeyPressed(Keyboard.Key.D))
        {
           Console.WriteLine("Pressed");
        }
    }
}