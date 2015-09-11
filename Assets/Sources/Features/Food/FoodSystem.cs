using Entitas;
using ICollectionOfEntityExtensions;
using System.Collections.Generic;

public class FoodSystem : IReactiveSystem, ISetPool
{
    Pool pool;

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
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        var position = pool.controllableEntity.position;
        ICollection<Entity> posEntities;
        pool.IsGameBoardPositionOpen(position, out posEntities);

        Entity food;
        if (posEntities != null &&
            posEntities.ContainsComponent(ComponentIds.Food, out food))
        {
            int points = food.food.points;
            pool.PlayAudio(food.audioPickupSource);
            pool.AddToFoodBag(points);
            pool.DestroyEntity(food);
        }
    }
}
