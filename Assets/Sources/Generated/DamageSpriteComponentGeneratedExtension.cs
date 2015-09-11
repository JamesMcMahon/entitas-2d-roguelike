using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public DamageSpriteComponent damageSprite { get { return (DamageSpriteComponent)GetComponent(ComponentIds.DamageSprite); } }

        public bool hasDamageSprite { get { return HasComponent(ComponentIds.DamageSprite); } }

        static readonly Stack<DamageSpriteComponent> _damageSpriteComponentPool = new Stack<DamageSpriteComponent>();

        public static void ClearDamageSpriteComponentPool() {
            _damageSpriteComponentPool.Clear();
        }

        public Entity AddDamageSprite(Sprite newSprite) {
            var component = _damageSpriteComponentPool.Count > 0 ? _damageSpriteComponentPool.Pop() : new DamageSpriteComponent();
            component.sprite = newSprite;
            return AddComponent(ComponentIds.DamageSprite, component);
        }

        public Entity ReplaceDamageSprite(Sprite newSprite) {
            var previousComponent = hasDamageSprite ? damageSprite : null;
            var component = _damageSpriteComponentPool.Count > 0 ? _damageSpriteComponentPool.Pop() : new DamageSpriteComponent();
            component.sprite = newSprite;
            ReplaceComponent(ComponentIds.DamageSprite, component);
            if (previousComponent != null) {
                _damageSpriteComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveDamageSprite() {
            var component = damageSprite;
            RemoveComponent(ComponentIds.DamageSprite);
            _damageSpriteComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherDamageSprite;

        public static AllOfMatcher DamageSprite {
            get {
                if (_matcherDamageSprite == null) {
                    _matcherDamageSprite = new Matcher(ComponentIds.DamageSprite);
                }

                return _matcherDamageSprite;
            }
        }
    }
}
