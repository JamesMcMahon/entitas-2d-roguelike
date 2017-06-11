using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;

[Pool]
[Unique]
public class SpriteCacheComponent : IComponent
{
    public IDictionary<string, UnityEngine.Sprite> value;
}
