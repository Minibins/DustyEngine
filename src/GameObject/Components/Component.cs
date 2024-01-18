
public class Component
{
    private GameObject gameObject;

    public GameObject GameObject
    {
        get => gameObject;
        set
        {
            if(gameObject!=null) gameObject.Components.Remove(this);
            gameObject = value;
        }
    }
}
