using System;

namespace DustyEngine
{
    internal static class Program
    {
        private static void Main()
        {
            Core core = new Core();

            if (core == null)
            {
                throw new Exception("Error:" + "Core was not create");
            }
        }
    }
}