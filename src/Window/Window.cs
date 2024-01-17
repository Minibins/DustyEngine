using System;
using SFML.Graphics;
using SFML.Window;
public class Window
{
    private Scene scene;
    private RenderWindow window;
    private uint width, height, fpsLimit;
    private String title;

    public Window(uint width,uint height,uint fpsLimit,String title,Scene scene)
    {
        this.scene = scene;
        this.width = width;
        this.height = height;
        this.fpsLimit = fpsLimit;
        this.title = title;
        CreateWindow();
    }

    private void CreateWindow()
    {
        window = new RenderWindow(new VideoMode(width,height),title);
        window.SetFramerateLimit(fpsLimit);
        window.Closed += OnClosed;

        
    }

    public void Run()
    {
        window.DispatchEvents();

        window.Clear();

        foreach(var gameObject in Scene.GameObjects)
        {
            if(gameObject.GetType() == typeof(GameObject)) (gameObject as GameObject).Draw(window);
            }

        window.Display();
    }

    private void OnClosed(object sender,EventArgs e)
    {
        window.Close();
    }
}