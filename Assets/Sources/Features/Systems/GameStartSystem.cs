using Entitas;

public class GameStartSystem : IInitializeSystem
{
    readonly PoolContext pool;

    public GameStartSystem(Contexts contexts)
    {
        pool = contexts.pool;
    }

    void IInitializeSystem.Initialize()
    {
        pool.SetFoodBag(100);
        pool.SetConfig(
            DefaultConfigConstants.COLUMNS,
            DefaultConfigConstants.ROWS,
            DefaultConfigConstants.FOOD_COUNT_MIN,
            DefaultConfigConstants.FOOD_COUNT_MAX,
            DefaultConfigConstants.WALL_COUNT_MIN,
            DefaultConfigConstants.WALL_COUNT_MAX,
            DefaultConfigConstants.ENEMY_COUNT_MULTIPLIER,
            DefaultConfigConstants.FOOD_POINTS,
            DefaultConfigConstants.SODA_POINTS,
            DefaultConfigConstants.ENEMY1_DMG,
            DefaultConfigConstants.ENEMY2_DMG,
            DefaultConfigConstants.TURN_DELAY
        );
    }
}
