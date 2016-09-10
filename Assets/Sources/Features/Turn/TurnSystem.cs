using Entitas;
using ICollectionExtensions;
using LinkedListExtensions;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TurnSystem : IMultiReactiveSystem, ISetPool
{
    LinkedList<Entity> turnOrder = new LinkedList<Entity>();
    LinkedListNode<Entity> currentTurnNode;
    Pool pool;
    Group turnBasedEntities;

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;

        turnBasedEntities = pool.GetGroup(Matcher.TurnBased);
        turnBasedEntities.OnEntityAdded += OnTurnBasedEntityAdded;
        turnBasedEntities.OnEntityRemoved += OnTurnBasedEntityRemoved;

        // reset current node when level is reset
        pool.GetGroup(Matcher.LevelTransitionDelay).OnEntityRemoved += 
            (group, entity, index, component) => currentTurnNode = null;
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

        if (turnOrder.Empty())
        {
            return; // nothing to do
        }

        currentTurnNode = currentTurnNode == null ?
            turnOrder.First :
            currentTurnNode.NextOrFirst();
        var nextEntity = currentTurnNode.Value;
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
        if (turnOrder.Empty())
        {
            turnOrder.AddFirst(entity);
            return;
        }

        var newIndex = entity.turnBased.index;
        var firstIndex = turnOrder.First.Value.turnBased.index;
        if (firstIndex >= newIndex)
        {
            turnOrder.AddFirst(entity);
            return;
        }

        var lastIndex = turnOrder.Last.Value.turnBased.index;
        if (lastIndex <= newIndex)
        {
            turnOrder.AddLast(entity);
            return;
        }

        var match = turnOrder.Nodes()
            .FirstOrDefault(n => n.Next.Value.turnBased.index >= newIndex);
        turnOrder.AddAfter(match, entity);
    }

    void OnTurnBasedEntityRemoved(Group group, Entity entity, int index,
                                  IComponent component)
    {
        turnOrder.Remove(entity);
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
