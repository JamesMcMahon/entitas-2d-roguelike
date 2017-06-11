using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Pool]
[Unique]
public class ViewContainerComponent : IComponent
{
    public Transform value;
}
