using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;

[Pool]
[Unique]
public class CurrentTurnNodeComponent : IComponent
{
    public LinkedListNode<PoolEntity> value;
}
