using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Game  
{
    private GameObject obj;
    
    public void Start()
    {
        obj = new();
        
        Scene.Instantiate(obj);

        obj.color = Color.Green;

        obj.transform.Position = new(50, 350, 0);
        obj.transform.Scale = new(50, 50, 0);
        obj.AddComponent(new CharacterController(4f));

        GameObject obj1 = new();

        Scene.Instantiate(obj1);

        obj1.color = Color.White;

        obj1.transform.Position = new(20, 400, 0);
        obj1.transform.Scale = new(600, 50, 0);
    }
}