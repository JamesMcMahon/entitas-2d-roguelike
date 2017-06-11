using Entitas;

[Pool]
public class AnimationComponent : IComponent
{
    public Animation animation;

    public string name
    {
        get { return animation.ToString(); }
    }
}
