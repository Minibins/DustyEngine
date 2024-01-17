using System.Collections.Generic;

using static System.Formats.Asn1.AsnWriter;
public class Scene : MessagesRestreamer
{
    static Window  window;
    private static Scene instance;
    public Scene() 
    {
        instance = this;
        
            window = new Window(640,480,60,"test",instance);
        }
    static public List<object> GameObjects => instance.children;
 // {
 //     get=>instance.children.OfType<GameObject>().ToList();
 //     set => instance.children = value.OfType<object>().ToList();
 // }
    public static void Instantiate(GameObject obj)
    { 
        GameObjects.Add(obj);
    }
    public static void Start()
    { 
    }
    public static void Update() 
    {
        window.Run();
    }
}