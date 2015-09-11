using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpriteSystem : IReactiveSystem, IInitializeSystem
{
    Dictionary<string, UnityEngine.Sprite> spriteCache = new Dictionary<string, UnityEngine.Sprite>();

    TriggerOnEvent IReactiveSystem.trigger
    {
        get
        {
            return Matcher.AllOf(Matcher.DamageSprite, Matcher.View,
                                 Matcher.Destructible).OnEntityAdded();
        }
    }

    void IInitializeSystem.Initialize()
    {
        UnityEngine.Sprite[] sprites = Resources.LoadAll<UnityEngine.Sprite>("Sprites");
        foreach (var s in sprites)
        {
            spriteCache.Add(s.name, s);
        }
    }

    void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            if (!e.destructible.IsDamaged)
            {
                continue;
            }

            var gameObject = e.view.gameObject;
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                if (spriteRenderer.sprite.name != e.damageSprite.name)
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
}
