using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;

[SingleEntity]
public class SpriteCacheComponent : IComponent
{
    public IDictionary<string, UnityEngine.Sprite> value;
}
