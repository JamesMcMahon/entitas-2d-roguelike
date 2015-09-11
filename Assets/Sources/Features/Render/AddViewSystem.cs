using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AddViewSystem : IReactiveSystem
{
    Dictionary<string, Transform> nestedViewContainers = new Dictionary<string, Transform>();

    TriggerOnEvent IReactiveSystem.trigger
    {
        get { return Matcher.Resource.OnEntityAdded(); }
    }

    readonly Transform viewContainer = new GameObject("Views").transform;

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            var resName = "Prefabs/" + e.resource.name;
            var res = Resources.Load<GameObject>(resName);
            GameObject gameObject = null;
            try
            {
                gameObject = UnityEngine.Object.Instantiate(res);
            }
            catch (Exception)
            {
                Debug.Log("Cannot instantiate " + resName);
            }
            if (gameObject == null)
            {
                continue;
            }

            var parent = e.hasNestedView ? GetNested(e.nestedView.name) : viewContainer;
            gameObject.transform.SetParent(parent, false);
            e.AddView(gameObject);

            if (e.hasPosition)
            {
                var pos = e.position;
                gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
            }
        }
    }

    Transform GetNested(string name)
    {
        if (nestedViewContainers.ContainsKey(name))
        {
            return nestedViewContainers[name];
        }

        var nestedView = new GameObject(name).transform;
        nestedView.SetParent(viewContainer, false);
        nestedViewContainers[name] = nestedView;
        return nestedView;
    }
}