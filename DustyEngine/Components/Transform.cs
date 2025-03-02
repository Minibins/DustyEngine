namespace DustyEngine.Components
{
    public class Transform : Component
    {
        public Transform? ParentTransform { get; set; }

        public Vector3 Position { get; set; } = new Vector3();
        public Vector3 LocalPosition { get; set; } = new Vector3();


        public void OnEnable()
        {
            if (ParentTransform != null)
            {
                ParentTransform = Parent.GetComponent<Transform>();
                
            }
        }

        public void Start()
        {
            //  Console.WriteLine($"{Parent.Name} position: global: {GlobalPosition}, local: {LocalPosition}");
        }
    }

    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3()
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
}