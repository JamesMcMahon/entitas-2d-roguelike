using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : IReactiveSystem
{
    TriggerOnEvent IReactiveSystem.trigger
    {
        get
        {
            return Matcher.AllOf(Matcher.Animation, Matcher.View).OnEntityAdded();
        }
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
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
