using Entitas;
using Entitas.CodeGeneration.Attributes;

[Pool]
[Unique]
public class GameBoardComponent : IComponent
{
    public int columns;
    public int rows;
}
