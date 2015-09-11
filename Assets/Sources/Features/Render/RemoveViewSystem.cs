using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class RemoveViewSystem : IReactiveSystem, ISetPool, IEnsureComponents
{
    TriggerOnEvent IReactiveSystem.trigger
    {
        get { return Matcher.Resource.OnEntityRemoved(); }
    }

    IMatcher IEnsureComponents.ensureComponents
    {
        get { return Matcher.View; }
    }

    void ISetPool.SetPool(Pool pool)
    {
        pool.GetGroup(Matcher.View).OnEntityRemoved += OnEntityRemoved;
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        Debug.Log("RemoveViewSystem");

        foreach (var e in entities)
        {
            e.RemoveView();
        }
    }

    void OnEntityRemoved(Group group, Entity entity, int index, IComponent component)
    {
        var viewComponent = (ViewComponent)component;
        var gameObject = viewComponent.gameObject;
        Object.Destroy(gameObject);
    }
}
