using DustyEngine.Components;
using System;

public class Player : Component
{
    public string Message { get; set; } = "Hello from DLL!";

    public void OnEnable()
    {
      //  Console.WriteLine("Execute OnEnable on: " + Parent.Name + " on " + GetType().Name);
    }

    public void OnDisable()
    {
      //  Console.WriteLine("Execute OnDisable on:" + Parent.Name + " " + GetType().Name);
    }

    public void Start()
    {
     //   Console.WriteLine("Execute Start on: " + Parent.Name + " on " + GetType().Name);
        
        Console.WriteLine(Parent.GetComponent<Transform>());
    }
}