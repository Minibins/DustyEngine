using SFML.Window;

namespace DustyEngine.Input
{
    public static class Input
    {
        public static bool IsKeyPressed(Keyboard.Key key)
        {
            return Keyboard.IsKeyPressed(key);
        }
    }
}