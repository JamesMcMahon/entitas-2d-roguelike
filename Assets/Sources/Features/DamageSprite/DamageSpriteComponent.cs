using Entitas;

[Pool]
public class DamageSpriteComponent : IComponent
{
    public Sprite sprite;

    public string name
    {
        get { return sprite.ToString(); }
    }
}
