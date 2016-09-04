using Entitas;
using UnityEngine;

public class TurnSystem : IExecuteSystem, ISetPool
{
    NonContiguousData<Entity> indexedEntities = new NonContiguousData<Entity>();
    Entity nextEntity;
    Pool pool;
    ActionTimer timer = new ActionTimer();
    Group turnBasedEntities;

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;

        turnBasedEntities = pool.GetGroup(Matcher.TurnBased);
        turnBasedEntities.OnEntityAdded += OnTurnBasedEntityAdded;
        turnBasedEntities.OnEntityRemoved += OnTurnBasedEntityRemoved;
    }

    void IExecuteSystem.Execute()
    {
        timer.Tick(Time.deltaTime);

        if (!timer.Done || pool.isActiveTurnBased || indexedEntities.Empty())
        {
            return; // nothing to do
        }

        nextEntity = indexedEntities.Next();
        var baseDelay = nextEntity.turnBased.delay;
        if (nextEntity.isSkipTurn)
        {
            // skip next turn but still fire delay timer to replicate
            // functionality of original game
            nextEntity.isSkipTurn = false;
            nextEntity = null;
        }

        // delay the next entity becoming active
        bool onlyEntity = turnBasedEntities.count < 2;
        var delayTime = onlyEntity ?  baseDelay * 2 : baseDelay;
        timer.Add(delayTime, () =>
        {
            if (nextEntity != null)
            {
                nextEntity.isActiveTurnBased = true;
                nextEntity = null;
            }
        });
    }

    void OnTurnBasedEntityAdded(Group group, Entity entity, int index,
                                IComponent component)
    {
        indexedEntities[entity.turnBased.index] = entity;
    }

    void OnTurnBasedEntityRemoved(Group group, Entity entity, int index,
                                  IComponent component)
    {
        indexedEntities.Remove(((TurnBasedComponent)component).index);
    }
}
