using System.Diagnostics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace DustyEngine.GraphicsEngineOpneGL;

public class GraphicsEngineOpenGl
{
    public void RunMainLoop(Action updateCallback)
    {
        Process process = Process.GetCurrentProcess();
        int processorCount = Environment.ProcessorCount;
        
        TimeSpan prevCpuTime = process.TotalProcessorTime;
        DateTime prevTime = DateTime.Now;
        
        
        Console.WriteLine("GraphicsEngineOpenGl is working");
      
        var nativeWindowSettings = new NativeWindowSettings()
        {
            Size = new Vector2i(800, 600),
            Title = Program.settings.ProjectName,
        };

        using var window = new GameWindow(GameWindowSettings.Default, nativeWindowSettings);
        
        ResourceMonitor.Init();
        
        window.UpdateFrame += (e) =>
        {
            updateCallback?.Invoke();
            ResourceMonitor.Update();
        };
        window.Run();
    }
}