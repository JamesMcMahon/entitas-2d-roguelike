using System;

public class ActionTimer
{
    Action action;
    float timer;

    public bool Done
    {
        get { return action == null; }
    }

    public void Add(float initialTime, Action action)
    {
        timer = initialTime;
        this.action = action;
    }

    public void Tick(float time)
    {
        if (Done)
        {
            return; // no-op
        }

        timer -= time; 
        if (timer <= 0)
        {
            action();
            action = null;
        }
    }
}
