using System;
using SFML.Graphics;
using SFML.Window;

namespace DustyEngine.Window
{
    public class Window
    {
        private Scene scene;
        private RenderWindow window;
        private uint width, height, fpsLimit;
        private String title;

        public Window(uint width, uint height, uint fpsLimit, String title, Scene scene)
        {
            this.scene = scene;
            this.width = width;
            this.height = height;
            this.fpsLimit = fpsLimit;
            this.title = title;


            System.Threading.Thread windowThread = new System.Threading.Thread(CreateWindow);
            windowThread.Start();
        }

        private void CreateWindow()
        {
            window = new RenderWindow(new VideoMode(this.width, this.height), this.title);
            window.SetFramerateLimit(this.fpsLimit);
            window.Closed += OnClosed;

            Run();
        }

        private void Run()
        {
            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear();

                foreach (var gameObject in scene.GameObjects)
                {
                    gameObject.Draw(window);
                }

                window.Display();
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}