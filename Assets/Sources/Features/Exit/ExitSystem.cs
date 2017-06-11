using Entitas;
using System.Collections.Generic;

public class ExitSystem : ReactiveSystem<PoolEntity>
{
    readonly PoolContext pool;
    readonly IGroup<PoolEntity> exitGroup;

    public ExitSystem(Contexts contexts)
        : base(contexts.pool)
    {
        pool = contexts.pool;
        exitGroup = pool.GetGroup(PoolMatcher.Exit);
    }

    protected override bool Filter(PoolEntity entity)
    {
        return true;
    }

    protected override ICollector<PoolEntity> GetTrigger(IContext<PoolEntity> context)
    {
        return context.CreateCollector(Matcher<PoolEntity>.AllOf(
                PoolMatcher.Controllable,
                PoolMatcher.Position));
    }

    protected override void Execute(List<PoolEntity> entities)
    {
        var controllablePos = pool.controllableEntity.position;

        foreach (var exit in exitGroup.GetEntities())
        {
            if (controllablePos.Equals(exit.position))
            {
                int currentLevel = pool.level.level;
                pool.ReplaceLevel(currentLevel + 1);
                break;
            }
        }
    }
}
