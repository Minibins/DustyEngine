using System.Text.Json.Serialization;

namespace DustyEngine.Components
{
    public class Transform : Component
    {
        private Vector3 _localPosition = new Vector3(0, 0, 0);

        public Vector3 LocalPosition
        {
            get => _localPosition;
            set { _localPosition = value; }
        }

        [JsonIgnore]
        public Vector3 GlobalPosition
        {
            get
            {
                if (Parent.Parent == null)
                    return _localPosition;

                var parentTransform = Parent.Parent.GetComponent<Transform>();
                return parentTransform != null ? parentTransform.GlobalPosition + _localPosition : _localPosition;
            }
        }


        public override string ToString()
        {
            return $"Local: {LocalPosition}, Global: {GlobalPosition}";
        }
    }
}


public class Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public Vector3() : this(0, 0, 0)
    {
    }

    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Vector3 operator +(Vector3 a, Vector3 b) =>
        new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3 operator -(Vector3 a, Vector3 b) =>
        new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public override string ToString() => $"({X}, {Y}, {Z})";
}