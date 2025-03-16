using DustyEngine.Components;
using System;

public class Player : Component
{
    public string Message { get; set; } = "Hello from DLL!";

    public void OnEnable()
    {
    }

    public void OnDisable()
    {
    }

    public void Start()
    {
        Console.WriteLine(Parent.GetComponent<Transform>());
    }
}