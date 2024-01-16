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

    public void Start()
    {
        scene = new Scene();
        scene.Instantiate(new GameObject(new Vector2f(100, 100), new Vector2f(50, 50), Color.Blue));

        Window window = new Window(640, 480, 60, "test", scene);

        scene.Instantiate(new GameObject(new Vector2f(200, 100), new Vector2f(50, 50), Color.Blue));
    }

    public void Update()
    {
        if (Input.IsKeyPressed(Keyboard.Key.Space))
        {
            scene.GameObjects[0].Velocity = new Vector2f(5, 0);
        }
    }
}