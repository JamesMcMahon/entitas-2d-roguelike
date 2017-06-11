using Entitas;

[Pool]
public class PositionComponent : IComponent
{
    public int x;
    public int y;

    public override bool Equals(object obj)
    {
        var other = obj as PositionComponent;
        return other != null && x == other.x && y == other.y;
    }

    public override int GetHashCode()
    {
        int hash = 13;
        hash = (hash * 7) + x.GetHashCode();
        hash = (hash * 7) + y.GetHashCode();
        return hash;
    }
}
