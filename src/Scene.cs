using System.Collections.Generic;

public class Scene : MessagesRestreamer
{
    private static Scene instance;

    public Scene()
    {
        instance = this;
    }

    public static List<object> GameObjects => instance.children;

    public static void Instantiate(GameObject obj)
    {
        GameObjects.Add(obj);
    }
}