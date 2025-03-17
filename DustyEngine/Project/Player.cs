using DustyEngine.Components;
using System;
using DustyEngine_V3;
using DustyEngine.Scene;

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
        Debug.Log(Parent.GetComponent<Transform>());
    }
}