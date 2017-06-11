using Entitas;
using System.Collections.Generic;

public class DestructibleSystem : ReactiveSystem<PoolEntity>
{
    readonly PoolContext pool;

    public DestructibleSystem(Contexts contexts)
        : base(contexts.pool)
    {
        pool = contexts.pool;
    }

    protected override bool Filter(PoolEntity entity)
    {
        return entity.hasDestructible;
    }

    protected override ICollector<PoolEntity> GetTrigger(
        IContext<PoolEntity> context)
    {
        return context.CreateCollector(PoolMatcher.Destructible);
    }

    protected override void Execute(List<PoolEntity> entities)
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
