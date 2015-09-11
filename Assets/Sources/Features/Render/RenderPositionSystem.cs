using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class RenderPositionSystem : IReactiveSystem, IExcludeComponents
{
    IMatcher IExcludeComponents.excludeComponents
    {
        get { return Matcher.SmoothMove; }
    }

    TriggerOnEvent IReactiveSystem.trigger
    {
        get
        {
            return Matcher.AllOf(Matcher.Position, Matcher.View).OnEntityAdded();
        }
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            var pos = e.position;
            e.view.gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
        }
    }
}
