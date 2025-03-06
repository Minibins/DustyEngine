using DustyEngine.Components;

namespace DustyEngine.Scene;

public class Scene
{
    public string Name { get; set; }
    public List<GameObject> GameObjects { get; set; } = new List<GameObject>();
    public List<Component> Components { get; set; } = new List<Component>();

    public void Instantiate(GameObject gameObject)
    {
        Console.WriteLine($"[DEBUG] Before Instantiate: GameObjects={GameObjects.Count}");
        GameObjects.Add(gameObject);
        gameObject.InvokeMethodInComponents("OnEnable");
        gameObject.InvokeMethodInComponents("Start");
        Console.WriteLine($"[DEBUG] After Instantiate: GameObjects={GameObjects.Count}");
    }
    public void Instantiate(GameObject gameObject, Transform transform)
    {
        Console.WriteLine($"[DEBUG] Before Instantiate: GameObjects={GameObjects.Count}");
        
        Transform targetTransform = gameObject.GetComponent<Transform>();
        if (targetTransform != null)
        {
            targetTransform.LocalPosition = transform.LocalPosition;
            targetTransform.LocalRotation = transform.LocalRotation;
            targetTransform.LocalScale= transform.LocalScale;
        }
        else
        {
            Console.WriteLine("[ERROR] GameObject has no Transform component!");
        }
        GameObjects.Add(gameObject);
        gameObject.InvokeMethodInComponents("OnEnable");
        gameObject.InvokeMethodInComponents("Start");
        Console.WriteLine($"[DEBUG] After Instantiate: GameObjects={GameObjects.Count}");
    }

    public void Destroy(GameObject gameObject)
    {
        Console.WriteLine($"[DEBUG] Before Destroy: GameObjects={GameObjects.Count}");
        if (GameObjects.Remove(gameObject))
        {
            gameObject.Destroy();
        }

        Console.WriteLine($"[DEBUG] After Destroy: GameObjects={GameObjects.Count}");
    }
}