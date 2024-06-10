
public class Component
{
    private Object _object;

    public Object Object
    {
        get => _object;
        set
        {
            if(_object!=null) _object.Components.Remove(this);
            _object = value;
        }
    }
}
