using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public TurnBasedComponent turnBased { get { return (TurnBasedComponent)GetComponent(ComponentIds.TurnBased); } }

        public bool hasTurnBased { get { return HasComponent(ComponentIds.TurnBased); } }

        static readonly Stack<TurnBasedComponent> _turnBasedComponentPool = new Stack<TurnBasedComponent>();

        public static void ClearTurnBasedComponentPool() {
            _turnBasedComponentPool.Clear();
        }

        public Entity AddTurnBased(int newIndex, float newDelay) {
            var component = _turnBasedComponentPool.Count > 0 ? _turnBasedComponentPool.Pop() : new TurnBasedComponent();
            component.index = newIndex;
            component.delay = newDelay;
            return AddComponent(ComponentIds.TurnBased, component);
        }

        public Entity ReplaceTurnBased(int newIndex, float newDelay) {
            var previousComponent = hasTurnBased ? turnBased : null;
            var component = _turnBasedComponentPool.Count > 0 ? _turnBasedComponentPool.Pop() : new TurnBasedComponent();
            component.index = newIndex;
            component.delay = newDelay;
            ReplaceComponent(ComponentIds.TurnBased, component);
            if (previousComponent != null) {
                _turnBasedComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveTurnBased() {
            var component = turnBased;
            RemoveComponent(ComponentIds.TurnBased);
            _turnBasedComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherTurnBased;

        public static AllOfMatcher TurnBased {
            get {
                if (_matcherTurnBased == null) {
                    _matcherTurnBased = new Matcher(ComponentIds.TurnBased);
                }

                return _matcherTurnBased;
            }
        }
    }
}
