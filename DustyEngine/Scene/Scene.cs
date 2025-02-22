using System.Reflection;
using DustyEngine.Components;

namespace DustyEngine.Scene;

public class Scene
{
    public string Name { get; set; }
    public List<GameObject> GameObjects { get; set; } = new List<GameObject>();
    public List<Component> Components { get; set; } = new List<Component>();
}