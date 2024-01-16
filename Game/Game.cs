using DustyEngine;
using DustyEngine.GameObject;
using SFML.Graphics;
using SFML.System;
using Window = DustyEngine.Window.Window;

public class Game
{
    private Scene scene;
    private Window window;

    public void Start()
    {
        scene = new Scene();
        scene.Instantiate(new GameObject(new Vector2f(100, 100), new Vector2f(50, 50), Color.Blue));
        
        window = new Window(640, 480, 60, "test", scene);

        scene.Instantiate(new GameObject(new Vector2f(200, 100), new Vector2f(50, 50), Color.Red));
    }
    
}