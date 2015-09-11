using Entitas;
using System.Linq;

public class AudioComponent : IComponent
{
    public Audio[] clips;
    public bool randomizePitch;

    public string[] clipNames
    {
        get { return clips.Select(i => i.ToString()).ToArray(); }
    }
}
