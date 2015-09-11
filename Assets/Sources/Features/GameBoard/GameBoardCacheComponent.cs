using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;

[SingleEntity]
public class GameBoardCacheComponent : IComponent
{
    public ICollection<Entity>[,] grid;
}
