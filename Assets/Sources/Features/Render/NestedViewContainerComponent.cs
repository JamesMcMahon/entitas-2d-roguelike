using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;
using UnityEngine;

[Pool]
[Unique]
public class NestedViewContainerComponent : IComponent
{
    public IDictionary<string, Transform> value;
}
