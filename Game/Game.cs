using SFML.Graphics;
using SFML.System;

public class Game  
{
    private GameObject _player;
    
    public void Start()
    {
        _player = new GameObject();
        
        Scene.Instantiate(_player);

        _player.Color = Color.Green;

        _player.Transform.Position = new Vector2f(50, 350);
        _player.Transform.Scale = new Vector2f(50, 50);
        _player.AddComponent(new CharacterController(4f));

        GameObject platform = new GameObject();

        Scene.Instantiate(platform);

        platform.Color = Color.White;

        platform.Transform.Position = new Vector2f(20, 400);
        platform.Transform.Scale = new Vector2f(600, 50);
    }
}