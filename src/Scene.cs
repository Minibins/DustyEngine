using System.Collections.Generic;

public class Scene : MessagesRestreamer
{
    private static Scene instance;

    public Scene(string file)
    {
        if(!file.EndsWith(".scen")) throw new Exception("Пошёл в жопу со своим " + file + " Я принимаю только .scen");
        
        instance = this;
        List<string> lines = FileReader.Read(file);
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