using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class RemoveViewSystem : ReactiveSystem<PoolEntity>
{
    public RemoveViewSystem(Contexts contexts)
        : base(contexts.pool)
    {
        contexts.pool.GetGroup(PoolMatcher.View).OnEntityRemoved += OnEntityRemoved;
    }

    protected override bool Filter(PoolEntity entity)
    {
        return entity.hasView;
    }

    protected override ICollector<PoolEntity> GetTrigger(IContext<PoolEntity> context)
    {
        return new Collector<PoolEntity>(
            new []
            { 
                context.GetGroup(PoolMatcher.Resource),
            },
            new []
            { 
                GroupEvent.Removed
            }
        );    
    }

    protected override void Execute(List<PoolEntity> entities)
    {
        Debug.Log("RemoveViewSystem");

        foreach (var e in entities)
        {
            e.RemoveView();
        }
    }

    void OnEntityRemoved(IGroup<PoolEntity> group, Entity entity, int index,
                         IComponent component)
    {
        var viewComponent = (ViewComponent)component;
        var gameObject = viewComponent.gameObject;
        Object.Destroy(gameObject);
    }
}
