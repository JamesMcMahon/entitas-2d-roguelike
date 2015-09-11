using Entitas;
using System.Collections.Generic;

public class DestructibleSystem : IReactiveSystem, ISetPool
{
    Pool pool;

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;
    }

    TriggerOnEvent IReactiveSystem.trigger
    {
        get
        {
            return Matcher.Destructible.OnEntityAdded();
        }
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            if (e.destructible.hp <= 0)
            {
                pool.DestroyEntity(e);
            }
        }
    }
}
