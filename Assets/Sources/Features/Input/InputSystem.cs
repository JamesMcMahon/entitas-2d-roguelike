using Entitas;
using ICollectionOfEntityExtensions;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : IReactiveSystem, ISetPool
{
    static Vector2 ToVector(Movement movement)
    {
        switch (movement)
        {
            case Movement.Up:
                return new Vector2(0, 1);
            case Movement.Right:
                return new Vector2(1, 0);
            case Movement.Down:
                return new Vector2(0, -1);
            case Movement.Left:
            default:
                return new Vector2(-1, 0);
        }
    }

    Pool pool;

    TriggerOnEvent IReactiveSystem.trigger
    {
        get { return Matcher.MoveInput.OnEntityAdded(); }
    }

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        if (pool.isGameOver
            || pool.isLevelTransitionDelay
            || !pool.hasMoveInput
            || !pool.isControllable
            || !pool.controllableEntity.isActiveTurnBased)
        {
            // ignore input
            return;
        }

        var controllable = pool.controllableEntity;
        pool.ReplaceFoodBag(pool.foodBag.points - 1);

        var movement = pool.moveInput.movement;
        var movementPos = ToVector(movement);

        var currentPos = controllable.position;
        int newX = currentPos.x + (int)movementPos.x;
        int newY = currentPos.y + (int)movementPos.y;

        ICollection<Entity> existing;
        bool canMove = pool.IsGameBoardPositionOpen(newX, newY, out existing);
        if (existing != null)
        {
            canMove = PrepareMove(controllable, existing);
        }

        if (canMove)
        {
            pool.PlayAudio(controllable.audioWalkSource);
            controllable.ReplacePosition(newX, newY);
        }

        controllable.isActiveTurnBased = false;
    }

    bool PrepareMove(Entity player, ICollection<Entity> entitiesInSpot)
    {
        if (entitiesInSpot.ContainsComponent(ComponentIds.AIMove))
        {
            // enemy there, can't do anything
            return false;
        }

        // handle walls
        Entity wall = null;
        if (entitiesInSpot.ContainsComponent(ComponentIds.Destructible, out wall))
        {
            wall.DamageDestructible();
            pool.PlayAudio(player.audioAttackSource);

            if (player.hasView)
            {
                player.AddAnimation(Animation.playerChop);
            }
            // nothing to do now that we've chopped
            return false;
        }

        // otherwise we can move
        return true;
    }
}
