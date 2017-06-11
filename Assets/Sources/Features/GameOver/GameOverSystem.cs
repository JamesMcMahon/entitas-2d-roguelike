using Entitas;
using System.Collections.Generic;

public class GameOverSystem : ReactiveSystem<PoolEntity>
{
    readonly PoolContext pool;

    public GameOverSystem(Contexts contexts)
        : base(contexts.pool)
    {
        pool = contexts.pool;
    }

    protected override bool Filter(PoolEntity entity)
    {
        return true;
    }

    protected override ICollector<PoolEntity> GetTrigger(IContext<PoolEntity> context)
    {
        return context.CreateCollector(PoolMatcher.FoodBag);
    }

    protected override void Execute(List<PoolEntity> entities)
    {
        if (!pool.isGameOver && pool.hasFoodBag && pool.foodBag.points <= 0)
        {
            pool.isGameOver = true;
            pool.PlayAudio(pool.controllableEntity.audioDeathSource);
        }
    }
}
