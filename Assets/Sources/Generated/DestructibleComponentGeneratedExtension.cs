using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public DestructibleComponent destructible { get { return (DestructibleComponent)GetComponent(ComponentIds.Destructible); } }

        public bool hasDestructible { get { return HasComponent(ComponentIds.Destructible); } }

        static readonly Stack<DestructibleComponent> _destructibleComponentPool = new Stack<DestructibleComponent>();

        public static void ClearDestructibleComponentPool() {
            _destructibleComponentPool.Clear();
        }

        public Entity AddDestructible(int newHp, int newStartingHP) {
            var component = _destructibleComponentPool.Count > 0 ? _destructibleComponentPool.Pop() : new DestructibleComponent();
            component.hp = newHp;
            component.startingHP = newStartingHP;
            return AddComponent(ComponentIds.Destructible, component);
        }

        public Entity ReplaceDestructible(int newHp, int newStartingHP) {
            var previousComponent = hasDestructible ? destructible : null;
            var component = _destructibleComponentPool.Count > 0 ? _destructibleComponentPool.Pop() : new DestructibleComponent();
            component.hp = newHp;
            component.startingHP = newStartingHP;
            ReplaceComponent(ComponentIds.Destructible, component);
            if (previousComponent != null) {
                _destructibleComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveDestructible() {
            var component = destructible;
            RemoveComponent(ComponentIds.Destructible);
            _destructibleComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherDestructible;

        public static AllOfMatcher Destructible {
            get {
                if (_matcherDestructible == null) {
                    _matcherDestructible = new Matcher(ComponentIds.Destructible);
                }

                return _matcherDestructible;
            }
        }
    }
}
