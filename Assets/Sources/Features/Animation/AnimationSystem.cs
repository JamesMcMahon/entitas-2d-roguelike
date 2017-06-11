using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : ReactiveSystem<PoolEntity>
{
    public AnimationSystem(Contexts contexts)
        : base(contexts.pool)
    {
    }

    protected override bool Filter(PoolEntity entity)
    {
        return entity.hasAnimation && entity.hasView;
    }

    protected override ICollector<PoolEntity> GetTrigger(IContext<PoolEntity> context)
    {
        return new Collector<PoolEntity>(
            new []
            { context.GetGroup(Matcher<PoolEntity>.AllOf(
                        PoolMatcher.Animation,
                        PoolMatcher.View))
            },
            new [] { GroupEvent.Added }
        );
    }

    protected override void Execute(List<PoolEntity> entities)
    {
        foreach (var e in entities)
        {
            var animation = e.animation.name;
            var gameObject = e.view.gameObject;

            var animator = gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                // we are using triggers only so we can assume SetTrigger
                animator.SetTrigger(animation);
            }

            // animations are single use so destroy after using
            e.RemoveAnimation();
        }
    }
}
