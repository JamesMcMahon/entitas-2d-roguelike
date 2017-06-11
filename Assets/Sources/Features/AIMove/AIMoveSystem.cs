using Entitas;
using ICollectionExtensions;
using ICollectionOfEntityExtensions;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveSystem : ReactiveSystem<PoolEntity>
{
    readonly PoolContext pool;

    public AIMoveSystem(Contexts contexts)
        : base(contexts.pool)
    {
        pool = contexts.pool;
    }

    protected override bool Filter(PoolEntity entity)
    {
        return entity.isActiveTurnBased && entity.isAIMove
        && entity.hasTurnBased && entity.hasPosition;
    }

    protected override ICollector<PoolEntity> GetTrigger(IContext<PoolEntity> context)
    {
        return new Collector<PoolEntity>(
            new []
            { context.GetGroup(Matcher<PoolEntity>.AllOf(
                        PoolMatcher.ActiveTurnBased,
                        PoolMatcher.AIMove,
                        PoolMatcher.TurnBased,
                        PoolMatcher.Position))
            },
            new [] { GroupEvent.Added }
        );
    }

    protected override void Execute(List<PoolEntity> entities)
    {
        var movingEntity = entities.SingleEntity<PoolEntity>();
        var target = pool.aIMoveTargetEntity;
        if (target == null || !target.hasPosition)
        {
            return;
        }

        var targetPos = target.position;
        var currentPos = movingEntity.position;
        var moveX = 0;
        var moveY = 0;

        bool moveYish = Mathf.Abs(targetPos.x - currentPos.x) == 0;
        if (moveYish)
        {
            moveY = targetPos.y > currentPos.y ? 1 : -1;
        }
        else
        {
            moveX = targetPos.x > currentPos.x ? 1 : -1;
        }
        int newX = currentPos.x + moveX;
        int newY = currentPos.y + moveY;

        ICollection<PoolEntity> existing;
        bool canMove = pool.IsGameBoardPositionOpen(newX, newY, out existing);
        if (existing != null && !existing.Empty())
        {
            canMove = PrepareMove(movingEntity, existing);
        }

        if (canMove)
        {
            movingEntity.ReplacePosition(newX, newY);
        }

        // skip next turn
        movingEntity.isSkipTurn = true;
        movingEntity.isActiveTurnBased = false;
    }


    bool PrepareMove(PoolEntity enemy, ICollection<PoolEntity> entitiesInSpot)
    {
        // handle player
        PoolEntity player;
        if (enemy.hasFoodDamager &&
            entitiesInSpot.ContainsComponent(PoolComponentsLookup.Controllable, out player))
        {
            pool.AddToFoodBag(-enemy.foodDamager.points);
            pool.PlayAudio(enemy.audioAttackSource);
            enemy.AddAnimation(Animation.enemyAttack);
            player.AddAnimation(Animation.playerHit);
            // can't move
            return false;
        }

        if (entitiesInSpot.Count == 1 &&
            entitiesInSpot.ContainsComponent(PoolComponentsLookup.Food))
        {
            return true;
        }

        return false;
    }
}
