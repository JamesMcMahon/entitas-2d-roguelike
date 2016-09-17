using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;
using UnityEngine;

[SingleEntity]
public class NestedViewContainerComponent : IComponent
{
    public IDictionary<string, Transform> value;
}
