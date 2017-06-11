using Entitas;
using Entitas.CodeGeneration.Attributes;

[Pool]
[Unique]
public class LevelComponent : IComponent
{
    public int level;
}
