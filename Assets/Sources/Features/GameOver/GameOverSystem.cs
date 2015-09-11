using Entitas;
using System.Collections.Generic;

public class GameOverSystem : IReactiveSystem, ISetPool
{
    Pool pool;

    TriggerOnEvent IReactiveSystem.trigger
    {
        get { return Matcher.FoodBag.OnEntityAdded(); }
    }

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        if (!pool.isGameOver && pool.hasFoodBag && pool.foodBag.points <= 0)
        {
            pool.isGameOver = true;
            pool.PlayAudio(pool.controllableEntity.audioDeathSource);
        }
    }
}
