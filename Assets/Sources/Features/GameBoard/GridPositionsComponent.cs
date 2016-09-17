using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;
using System.Collections.Generic;

[SingleEntity]
public class GridPositionsComponent : IComponent
{
    public IList<Vector2> value;
}
