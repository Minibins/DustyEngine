using System;

namespace DustyEngine
{
    internal static class Program
    {
        private static void Main()
        {
            Window window = new Window(640, 480, 60, "test");
            if (window == null)
            {
                throw new Exception("Error:" + "Window was not create");
            }
            
            Core core = new Core();
            
            if (core == null)
            {
                throw new Exception("Error:" + "Core was not create");
            }
        }
    }
}