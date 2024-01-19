using System;
using System.Collections.Generic;
using System.Linq;

public class Scene : MessagesRestreamer
{
    private static Scene _instance;

    public Scene(string file)
    {
        if (!file.EndsWith(".scen")) throw new Exception("Пошёл в жопу со своим " + file + " Я принимаю только .scen");

        _instance = this;
        List<string> _lines = FileReader.Read(file);
        
        Window window = new Window(640, 480, 60, "test");
        if (window == null)
        {
            throw new Exception("Error:" + "Window was not create");
        }

    }

    public static List<GameObject> GameObjects
    {
        get { return _instance._children.OfType<GameObject>().ToList(); }
    }

    public static void Instantiate(GameObject obj)
    {
        _instance._children.Add(obj);
    }
}