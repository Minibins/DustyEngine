using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Game
{
    public void Start()
    {        
        GameObject obj = new GameObject();
        
         Scene.Instantiate(obj);
         
         obj.color = Color.Green;
         
         obj.transform.Position = new Vector2f(50,350);
         obj.transform.Scale = new Vector2f(50,50);
         
         obj.UpdateObj();
         
         
         GameObject obj1 = new GameObject();

        Scene.Instantiate(obj1);
         
         obj1.color = Color.White;
         
         obj1.transform.Position = new Vector2f(20,400);
         obj1.transform.Scale = new Vector2f(600,50);
         
         obj1.UpdateObj();
         
    }


    public void Update()    
    {

    }
}