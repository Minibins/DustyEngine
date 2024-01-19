using System.Threading.Tasks;

public class Core : MessagesRestreamer
{
    public Core()
    {
      //  _children.Add(new Scene("C:/Users/Anton/Searches/Desktop/scene.scen"));
        _children.Add(new Scene("C:/Users/Maks/Desktop/scene.scen"));

        _children.Add(new Game());

        RestreamingMethod("Start");
        Task.Run(async () => FixedUpdate());
        Task.Run(async () => Update());
    }

    async void FixedUpdate()
    {
        while (true)
        {
            await Task.Delay(10);
            RestreamingMethod("FixedUpdate");
        }
    }

    private void Update()
    {
        while (true)
        {
            RestreamingMethod("Update");
        }
    }
}