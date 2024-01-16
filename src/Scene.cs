using System.Collections.Generic;

namespace DustyEngine
{
    public class Scene
    {
        public List<GameObject.GameObject> GameObjects = new List<GameObject.GameObject>();

        public void Instantiate(GameObject.GameObject obj)
        {
            GameObjects.Add(obj);
        }
    }
}