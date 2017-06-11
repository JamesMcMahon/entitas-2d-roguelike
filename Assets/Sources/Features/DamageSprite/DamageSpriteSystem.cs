using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpriteSystem : ReactiveSystem<PoolEntity>, IInitializeSystem
{
    readonly PoolContext pool;

    public DamageSpriteSystem(Contexts contexts)
        : base(contexts.pool)
    {
        pool = contexts.pool;
    }

    protected override bool Filter(PoolEntity entity)
    {
        return entity.hasDamageSprite && entity.hasView
        && entity.hasDamageSprite;
    }

    protected override ICollector<PoolEntity> GetTrigger(IContext<PoolEntity> context)
    {
        return context.CreateCollector(Matcher<PoolEntity>.AllOf(
                PoolMatcher.DamageSprite,
                PoolMatcher.View,
                PoolMatcher.Destructible));
    }

    void IInitializeSystem.Initialize()
    {
        var sprites = Resources.LoadAll<UnityEngine.Sprite>("Sprites");
        var spriteCache = new Dictionary<string, UnityEngine.Sprite>();
        foreach (var s in sprites)
        {
            spriteCache.Add(s.name, s);
        }
        pool.SetSpriteCache(spriteCache);
    }

    protected override void Execute(List<PoolEntity> entities)
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
