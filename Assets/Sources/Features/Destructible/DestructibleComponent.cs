using Entitas;

public class DestructibleComponent : IComponent
{
    public int hp;
    public int startingHP;

    public bool IsDamaged
    {
        get { return hp < startingHP;  }
    }
}
