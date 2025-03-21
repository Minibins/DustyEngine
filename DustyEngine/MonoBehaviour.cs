using DustyEngine.Components;

namespace DustyEngine;

public class MonoBehaviour : Behaviour
{
    protected GameObject gameObject => Parent;
    protected Transform transform => gameObject.GetComponent<Transform>();
}