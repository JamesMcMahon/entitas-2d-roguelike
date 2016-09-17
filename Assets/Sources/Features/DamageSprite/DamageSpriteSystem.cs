using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpriteSystem : ISetPool, IReactiveSystem, IInitializeSystem
{
    Pool pool;

    TriggerOnEvent IReactiveSystem.trigger
    {
        get
        {
            return Matcher.AllOf(Matcher.DamageSprite, Matcher.View,
                                 Matcher.Destructible).OnEntityAdded();
        }
    }

    void ISetPool.SetPool(Pool pool)
    {
        this.pool = pool;
    }

    void IInitializeSystem.Initialize()
    {
        UnityEngine.Sprite[] sprites = Resources.LoadAll<UnityEngine.Sprite>("Sprites");
        var spriteCache = new Dictionary<string, UnityEngine.Sprite>();
        foreach (var s in sprites)
        {
            spriteCache.Add(s.name, s);
        }
        pool.CreateEntity().AddSpriteCache(spriteCache);
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        var spriteCache = pool.spriteCache.value;
        foreach (var e in entities)
        {
            if (!e.destructible.IsDamaged)
            {
                continue;
            }

            var gameObject = e.view.gameObject;
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null &&
                spriteRenderer.sprite.name != e.damageSprite.name)
            {
                UnityEngine.Sprite sprite;
                bool available = spriteCache.TryGetValue(e.damageSprite.name, out sprite);
                if (available)
                {
                    spriteRenderer.sprite = sprite;
                }
                // no need to use this anymore so remove it
                e.RemoveDamageSprite();
            }
        }
    }
}
