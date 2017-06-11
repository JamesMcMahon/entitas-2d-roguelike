using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class RenderPositionSystem : ReactiveSystem<PoolEntity>
{
    public RenderPositionSystem(Contexts contexts)
        : base(contexts.pool)
    {
    }

    protected override bool Filter(PoolEntity entity)
    {
        return !entity.hasSmoothMove;
    }

    protected override ICollector<PoolEntity> GetTrigger(IContext<PoolEntity> context)
    {
        return context.CreateCollector(Matcher<PoolEntity>.AllOf(
            PoolMatcher.Position,
            PoolMatcher.View));
    }

    protected override void Execute(List<PoolEntity> entities)
    {
        foreach (var e in entities)
        {
            var pos = e.position;
            e.view.gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
        }
    }
}
