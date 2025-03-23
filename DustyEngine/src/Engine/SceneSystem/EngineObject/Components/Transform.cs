using System.Text.Json.Serialization;

namespace DustyEngine.Components
{
    public class Transform : Component
    {
        [JsonIgnore] private Vector3 _localPosition = new Vector3(0, 0, 0);
        [JsonIgnore] private Vector3 _localRotation = new Vector3(0, 0, 0);
        [JsonIgnore] private Vector3 _localScale = new Vector3(1, 1, 1);

        public Vector3 LocalPosition
        {
            get => _localPosition;
            set { _localPosition = value; }
        }

        public Vector3 LocalRotation
        {
            get => _localRotation;
            set { _localRotation = value; }
        }

        public Vector3 LocalScale
        {
            get => _localScale;
            set { _localScale = value; }
        }

        [JsonIgnore]public Vector3 GlobalPosition
        {
            get
            {
                if (Parent.Parent == null)
                    return _localPosition;

                var parentTransform = Parent.Parent.GetComponent<Transform>();
                return parentTransform != null ? parentTransform.GlobalPosition + _localPosition : _localPosition;
            }
        }

        [JsonIgnore] public Vector3 GlobalRotation
        {
            get
            {
                if (Parent.Parent == null)
                    return _localRotation;

                var parentTransform = Parent.Parent.GetComponent<Transform>();
                return parentTransform != null ? parentTransform.GlobalRotation + _localRotation : _localRotation;
            }
        }

        [JsonIgnore] public Vector3 GlobalScale
        {
            get
            {
                if (Parent.Parent == null)
                    return _localScale;

                var parentTransform = Parent.Parent.GetComponent<Transform>();
                return parentTransform != null
                    ? new Vector3(
                        parentTransform.GlobalScale.X * _localScale.X,
                        parentTransform.GlobalScale.Y * _localScale.Y,
                        parentTransform.GlobalScale.Z * _localScale.Z)
                    : _localScale;
            }
        }

        public override string ToString()
        {
            return $"Local: {LocalPosition}, Rotation: {LocalRotation}, Scale: {LocalScale}, " +
                   $"Global: {GlobalPosition}, GlobalRotation: {GlobalRotation}, GlobalScale: {GlobalScale}";
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

    public static Vector3 operator *(Vector3 a, Vector3 b) =>
        new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

    public override string ToString() => $"({X}, {Y}, {Z})";
}