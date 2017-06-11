using Entitas;
using ICollectionOfEntityExtensions;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : ReactiveSystem<PoolEntity>
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

    readonly PoolContext pool;

    public InputSystem(Contexts contexts)
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
        return context.CreateCollector(PoolMatcher.MoveInput);
    }

    protected override void Execute(List<PoolEntity> entities)
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

        ICollection<PoolEntity> existing;
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

    bool PrepareMove(PoolEntity player, ICollection<PoolEntity> entitiesInSpot)
    {
        if (entitiesInSpot.ContainsComponent(PoolComponentsLookup.AIMove))
        {
            // enemy there, can't do anything
            return false;
        }

        // handle walls
        PoolEntity wall = null;
        if (entitiesInSpot.ContainsComponent(PoolComponentsLookup.Destructible, out wall))
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
