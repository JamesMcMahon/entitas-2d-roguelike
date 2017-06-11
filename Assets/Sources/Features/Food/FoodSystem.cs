using Entitas;
using ICollectionOfEntityExtensions;
using System.Collections.Generic;

public class FoodSystem : ReactiveSystem<PoolEntity>
{
    readonly PoolContext pool;

    public FoodSystem(Contexts contexts)
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
        return context.CreateCollector(Matcher<PoolEntity>.AllOf(
                PoolMatcher.Controllable,
                PoolMatcher.Position));
    }

    protected override void Execute(List<PoolEntity> entities)
    {
        var position = pool.controllableEntity.position;
        ICollection<PoolEntity> posEntities;
        pool.IsGameBoardPositionOpen(position, out posEntities);

        PoolEntity food;
        if (posEntities != null &&
            posEntities.ContainsComponent(PoolComponentsLookup.Food, out food))
        {
            int points = food.food.points;
            pool.PlayAudio(food.audioPickupSource);
            pool.AddToFoodBag(points);
            pool.DestroyEntity(food);
        }
    }
}
