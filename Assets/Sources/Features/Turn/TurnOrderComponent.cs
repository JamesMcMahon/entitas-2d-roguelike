using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;

[SingleEntity]
public class TurnOrderComponent : IComponent
{
    public LinkedList<Entity> value;
}
