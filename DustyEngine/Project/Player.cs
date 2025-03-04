using DustyEngine.Components;
using System;

public class Player : Component
{
    public string Message { get; set; } = "Hello from DLL!";

    public void OnEnable()
    {
        Console.WriteLine(GetType().Name + " on " + Parent.Name);
        Console.WriteLine("OnEnable player");
    }

    public void OnDisable()
    {
        Console.WriteLine("OnDisable player");
    }

    public void Start()
    {
        Console.WriteLine("Starting player");

        Console.WriteLine(Parent.GetComponent<Transform>());
    }
}