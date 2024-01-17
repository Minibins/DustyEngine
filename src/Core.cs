using System;

public class Core : MessagesRestreamer
{

    public Core()
    {
        children.Add(new Scene());
        children.Add(new Game());
        
        RestreamingMethod("Start");
        while(true)
        {
            RestreamingMethod("Update");
        }
    }
}