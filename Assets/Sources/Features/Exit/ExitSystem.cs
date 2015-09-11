using Entitas;
using System.Collections.Generic;

public class ExitSystem : IReactiveSystem, ISetPool
{
    Pool pool;
    Group exitGroup;

    TriggerOnEvent IReactiveSystem.trigger
    {
        get
        {
            return Matcher.AllOf(Matcher.Controllable, Matcher.Position).OnEntityAdded();
        }
    }

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;
        exitGroup = pool.GetGroup(Matcher.Exit);
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        var controllablePos = pool.controllableEntity.position;

        foreach (var exit in exitGroup.GetEntities())
        {
            if (controllablePos.Equals(exit.position))
            {
                int currentLevel = pool.level.level;
                pool.ReplaceLevel(currentLevel + 1);
                break;
            }
        }
    }
}
