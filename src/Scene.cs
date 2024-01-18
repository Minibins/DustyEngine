using System.Collections.Generic;

public class Scene : MessagesRestreamer
{
    private static Scene instance;

    public Scene()
    {
        instance = this;
    }

    public static List<GameObject> GameObjects
    {
        get 
        {
            return instance.children.OfType<GameObject>().ToList();
        }
    }

    public static void Instantiate(GameObject obj)
    {
        instance.children.Add(obj);
    }
}