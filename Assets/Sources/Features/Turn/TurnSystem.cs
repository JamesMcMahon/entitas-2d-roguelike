using Entitas;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnSystem : IMultiReactiveSystem, ISetPool
{
    NonContiguousData<Entity> indexedEntities = new NonContiguousData<Entity>();
    Pool pool;
    Group turnBasedEntities;

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;

        turnBasedEntities = pool.GetGroup(Matcher.TurnBased);
        turnBasedEntities.OnEntityAdded += OnTurnBasedEntityAdded;
        turnBasedEntities.OnEntityRemoved += OnTurnBasedEntityRemoved;

        // reset index when level is reset
        pool.GetGroup(Matcher.LevelTransitionDelay).OnEntityRemoved += 
            (group, entity, index, component) => indexedEntities.Reset();
    }

    TriggerOnEvent[] IMultiReactiveSystem.triggers
    {
        get
        {
            return new []
            {
                Matcher.ActiveTurnBased.OnEntityRemoved(),
                Matcher.NextTurn.OnEntityAdded()
            };
        }
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        if (pool.isNextTurn)
        {
            pool.isNextTurn = false;
        }

        if (indexedEntities.Empty())
        {
            return; // nothing to do
        }

        var nextEntity = indexedEntities.Next();
        // delay the next entity becoming active
        var baseDelay = nextEntity.turnBased.delay;
        bool onlyEntity = turnBasedEntities.count < 2;
        var delayTime = onlyEntity ? baseDelay * 2 : baseDelay;
        pool.CreateEntity()
            .AddCoroutine(ActivateAfterDelay(delayTime, nextEntity));
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

    IEnumerator ActivateAfterDelay(float delayTime, Entity nextEntity)
    {
        var timer = delayTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        if (nextEntity.isSkipTurn)
        {
            // skip over but make sure that the turn system is retriggered
            nextEntity.isSkipTurn = false;
            pool.isNextTurn = true;
        }
        else
        {
            nextEntity.isActiveTurnBased = true;
        }
    }
}
