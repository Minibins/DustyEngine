using SFML.Window;

    public static class Input
    {
        public static bool IsKeyPressed(Keyboard.Key key)
        {
            return Keyboard.IsKeyPressed(key);
        }
    }