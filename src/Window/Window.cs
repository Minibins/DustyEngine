using System;
using SFML.Graphics;
using SFML.Window;

public class Window
{
    private RenderWindow renderWindow;
    private uint width, height, fpsLimit;
    private String title;

    public Window(uint width, uint height, uint fpsLimit, String title)
    {
        this.width = width;
        this.height = height;
        this.fpsLimit = fpsLimit;
        this.title = title;

        System.Threading.Thread windowThread = new System.Threading.Thread(CreateWindow);
        windowThread.Start();
    }

    private void CreateWindow()
    {
        renderWindow = new RenderWindow(new VideoMode(width, height), title);
        renderWindow.SetFramerateLimit(fpsLimit);
        renderWindow.Closed += OnClosed;

        Run();
    }

    private void Run()
    {
        while (true)
        {
            renderWindow.DispatchEvents();

            renderWindow.Clear();

            foreach (var gameObject in Scene.GameObjects)
            {
                if (gameObject.GetType() == typeof(GameObject)) (gameObject as GameObject).Draw(renderWindow);
            }

            renderWindow.Display();
        }
    }

    private void OnClosed(object sender, EventArgs e)
    {
        renderWindow.Close();
    }
}