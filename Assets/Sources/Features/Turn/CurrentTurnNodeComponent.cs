using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;

[SingleEntity]
public class CurrentTurnNodeComponent : IComponent
{
    public LinkedListNode<Entity> value;
}
