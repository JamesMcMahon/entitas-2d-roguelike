using Entitas;
using Entitas.CodeGeneration.Attributes;

[Pool]
[Unique]   
public class MoveInputComponent : IComponent
{
    public Movement movement;
}
