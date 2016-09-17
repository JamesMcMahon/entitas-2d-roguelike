using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AddViewSystem : ISetPool, IInitializeSystem, IReactiveSystem
{
    static Transform GetNested(string name,
                               Transform viewContainer,
                               IDictionary<string, Transform> nestedViewContainer)
    {
        if (nestedViewContainer.ContainsKey(name))
        {
            return nestedViewContainer[name];
        }

        var nestedView = new GameObject(name).transform;
        nestedView.SetParent(viewContainer, false);
        nestedViewContainer[name] = nestedView;
        return nestedView;
    }

    Pool pool;

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;
    }

    TriggerOnEvent IReactiveSystem.trigger
    {
        get { return Matcher.Resource.OnEntityAdded(); }
    }

    void IInitializeSystem.Initialize()
    {
        pool.CreateEntity().AddViewContainer(new GameObject("Views").transform);
        pool.CreateEntity().AddNestedViewContainer(new Dictionary<string, Transform>());
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        var viewContainer = pool.viewContainer.value;
        var nestedViewContainer = pool.nestedViewContainer.value;

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

            var parent = e.hasNestedView ?
                GetNested(e.nestedView.name, viewContainer, nestedViewContainer) :
                viewContainer;
            gameObject.transform.SetParent(parent, false);
            e.AddView(gameObject);

            if (e.hasPosition)
            {
                var pos = e.position;
                gameObject.transform.position = new Vector3(pos.x, pos.y, 0f);
            }
        }
    }
}
