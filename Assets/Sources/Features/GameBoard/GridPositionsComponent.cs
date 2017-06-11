using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using System.Collections.Generic;

[Pool]
[Unique]
public class GridPositionsComponent : IComponent
{
    public IList<Vector2> value;
}
