using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;

[Pool]
[Unique]
public class GameBoardCacheComponent : IComponent
{
    public ICollection<PoolEntity>[,] grid;
}
