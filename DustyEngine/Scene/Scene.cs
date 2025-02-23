using DustyEngine.Components;

namespace DustyEngine.Scene;

public class Scene
{
    public string Name { get; set; }
    public List<GameObject> GameObjects { get; set; } = new List<GameObject>();
    public List<Component> Components { get; set; } = new List<Component>();

    public void Instantiate(GameObject gameObject)
    {
        GameObjects.Add(gameObject);
    }

    public void Destroy(GameObject gameObject)
    {
        if (GameObjects.Remove(gameObject))
        {
            //     gameObject.Destroy();
        }
    }
}