using Entitas;

public class GameStartSystem : IInitializeSystem, ISetPool
{
    Pool pool;

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;
    }

    void IInitializeSystem.Initialize()
    {
        pool.SetFoodBag(100);
    }
}
