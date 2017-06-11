using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;

[Pool]
[Unique]
public class TurnOrderComponent : IComponent
{
    public LinkedList<PoolEntity> value;
}
