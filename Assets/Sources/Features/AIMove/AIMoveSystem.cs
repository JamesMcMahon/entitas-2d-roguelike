using Entitas;
using ICollectionExtensions;
using ICollectionOfEntityExtensions;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveSystem : IReactiveSystem, ISetPool, IEnsureComponents
{
    Pool pool;

    IMatcher IEnsureComponents.ensureComponents
    {
        get
        {
            return Matcher.AllOf(Matcher.ActiveTurnBased, Matcher.AIMove,
                                 Matcher.TurnBased, Matcher.Position);
        }
    }

    TriggerOnEvent IReactiveSystem.trigger
    {
        get
        {
            return Matcher.AllOf(Matcher.ActiveTurnBased, Matcher.AIMove,
                                 Matcher.TurnBased, Matcher.Position).OnEntityAdded();
        }
    }

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        var movingEntity = entities.SingleEntity();
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

        ICollection<Entity> existing;
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


    bool PrepareMove(Entity enemy, ICollection<Entity> entitiesInSpot)
    {
        // handle player
        Entity player;
        if (enemy.hasFoodDamager &&
            entitiesInSpot.ContainsComponent(ComponentIds.Controllable, out player))
        {
            pool.AddToFoodBag(-enemy.foodDamager.points);
            pool.PlayAudio(enemy.audioAttackSource);
            enemy.AddAnimation(Animation.enemyAttack);
            player.AddAnimation(Animation.playerHit);
            // can't move
            return false;
        }

        if (entitiesInSpot.Count == 1 &&
            entitiesInSpot.ContainsComponent(ComponentIds.Food))
        {
            return true;
        }

        return false;
    }
}
