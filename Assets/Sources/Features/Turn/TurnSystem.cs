using Entitas;
using ICollectionExtensions;
using LinkedListExtensions;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TurnSystem : IInitializeSystem, IMultiReactiveSystem, ISetPool
{
    Pool pool;
    Group turnBasedEntities;

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;

        turnBasedEntities = pool.GetGroup(Matcher.TurnBased);
        turnBasedEntities.OnEntityAdded += OnTurnBasedEntityAdded;
        turnBasedEntities.OnEntityRemoved += OnTurnBasedEntityRemoved;
    }

    void IInitializeSystem.Initialize()
    {
        pool.SetTurnOrder(new LinkedList<Entity>());
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

        var turnOrder = pool.turnOrder.value;
        if (turnOrder.Empty())
        {
            return; // nothing to do
        }

        if (pool.hasCurrentTurnNode)
        {
            pool.ReplaceCurrentTurnNode(pool.currentTurnNode.value.NextOrFirst());
        }
        else
        {
            pool.SetCurrentTurnNode(turnOrder.First);
        }

        var nextEntity = pool.currentTurnNode.value.Value;
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
        var turnOrder = pool.turnOrder.value;
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
        // get previous node before removing node from list
        var currentTurnNode = pool.currentTurnNode.value;
        var prevNode = currentTurnNode.PreviousOrLast();
        var removed = pool.turnOrder.value.Remove(entity);
        if (removed && currentTurnNode != null && currentTurnNode.Value == entity)
        {
            if (prevNode.List == null)
            {
                pool.RemoveCurrentTurnNode();
            }
            else
            {
                pool.ReplaceCurrentTurnNode(prevNode);
            }
        }
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
