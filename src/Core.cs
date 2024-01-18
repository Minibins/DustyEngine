using System;
public class Core : MessagesRestreamer
{

    public Core()
    {
        children.Add(new Scene("C:/Users/Anton/Searches/Desktop/scene.scen"));
        children.Add(new Game());

        RestreamingMethod("Start");
        Task.Run(async () => FixedUpdate());
        Task.Run(async ()=> Update());
    }
    async void FixedUpdate()
    {
        while(true)
        {
            await Task.Delay(10);
            RestreamingMethod("FixedUpdate");
        }
    }
    void Update()
    {
        while(true)
        {
            RestreamingMethod("Update");
        }
    }
}