using Entitas;
using ICollectionExtensions;
using LinkedListExtensions;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TurnSystem : ReactiveSystem<PoolEntity>, IInitializeSystem
{
    readonly PoolContext pool;
    IGroup<PoolEntity> turnBasedEntities;

    public TurnSystem(Contexts contexts)
        : base(contexts.pool)
    {
        pool = contexts.pool;

        turnBasedEntities = pool.GetGroup(PoolMatcher.TurnBased);
        turnBasedEntities.OnEntityAdded += OnTurnBasedEntityAdded;
        turnBasedEntities.OnEntityRemoved += OnTurnBasedEntityRemoved;
    }

    protected override bool Filter(PoolEntity entity)
    {
        return true;
    }

    protected override ICollector<PoolEntity> GetTrigger(IContext<PoolEntity> context)
    {
        return new Collector<PoolEntity>(
            new []
            { 
                context.GetGroup(PoolMatcher.ActiveTurnBased),
                context.GetGroup(PoolMatcher.NextTurn)
            },
            new []
            { 
                GroupEvent.Removed,
                GroupEvent.Added
            }
        );
    }

    void IInitializeSystem.Initialize()
    {
        pool.SetTurnOrder(new LinkedList<PoolEntity>());
    }

    protected override void Execute(List<PoolEntity> entities)
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

    void OnTurnBasedEntityAdded(IGroup<PoolEntity> group, PoolEntity entity,
                                int index, IComponent component)
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

    void OnTurnBasedEntityRemoved(IGroup<PoolEntity> group, PoolEntity entity,
                                  int index, IComponent component)
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

    IEnumerator ActivateAfterDelay(float delayTime, PoolEntity nextEntity)
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
