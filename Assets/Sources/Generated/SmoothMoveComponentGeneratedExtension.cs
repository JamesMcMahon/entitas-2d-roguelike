using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public SmoothMoveComponent smoothMove { get { return (SmoothMoveComponent)GetComponent(ComponentIds.SmoothMove); } }

        public bool hasSmoothMove { get { return HasComponent(ComponentIds.SmoothMove); } }

        static readonly Stack<SmoothMoveComponent> _smoothMoveComponentPool = new Stack<SmoothMoveComponent>();

        public static void ClearSmoothMoveComponentPool() {
            _smoothMoveComponentPool.Clear();
        }

        public Entity AddSmoothMove(float newMoveTime) {
            var component = _smoothMoveComponentPool.Count > 0 ? _smoothMoveComponentPool.Pop() : new SmoothMoveComponent();
            component.moveTime = newMoveTime;
            return AddComponent(ComponentIds.SmoothMove, component);
        }

        public Entity ReplaceSmoothMove(float newMoveTime) {
            var previousComponent = hasSmoothMove ? smoothMove : null;
            var component = _smoothMoveComponentPool.Count > 0 ? _smoothMoveComponentPool.Pop() : new SmoothMoveComponent();
            component.moveTime = newMoveTime;
            ReplaceComponent(ComponentIds.SmoothMove, component);
            if (previousComponent != null) {
                _smoothMoveComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveSmoothMove() {
            var component = smoothMove;
            RemoveComponent(ComponentIds.SmoothMove);
            _smoothMoveComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherSmoothMove;

        public static AllOfMatcher SmoothMove {
            get {
                if (_matcherSmoothMove == null) {
                    _matcherSmoothMove = new Matcher(ComponentIds.SmoothMove);
                }

                return _matcherSmoothMove;
            }
        }
    }
}
