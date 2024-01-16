using System;
using SFML.Graphics;
using SFML.Window;


namespace DustyEngine.Window
{
    public class Window
    {
        private Scene scene;
        private RenderWindow window;
        
        public Window(uint width, uint height, uint fpsLimit, String title,Scene scene)
        {
            window = new RenderWindow(new VideoMode(width, height), title);
            window.SetFramerateLimit(fpsLimit);

            this.scene = scene;
            
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
    }
}