using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public AnimationComponent animation { get { return (AnimationComponent)GetComponent(ComponentIds.Animation); } }

        public bool hasAnimation { get { return HasComponent(ComponentIds.Animation); } }

        static readonly Stack<AnimationComponent> _animationComponentPool = new Stack<AnimationComponent>();

        public static void ClearAnimationComponentPool() {
            _animationComponentPool.Clear();
        }

        public Entity AddAnimation(Animation newAnimation) {
            var component = _animationComponentPool.Count > 0 ? _animationComponentPool.Pop() : new AnimationComponent();
            component.animation = newAnimation;
            return AddComponent(ComponentIds.Animation, component);
        }

        public Entity ReplaceAnimation(Animation newAnimation) {
            var previousComponent = hasAnimation ? animation : null;
            var component = _animationComponentPool.Count > 0 ? _animationComponentPool.Pop() : new AnimationComponent();
            component.animation = newAnimation;
            ReplaceComponent(ComponentIds.Animation, component);
            if (previousComponent != null) {
                _animationComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveAnimation() {
            var component = animation;
            RemoveComponent(ComponentIds.Animation);
            _animationComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherAnimation;

        public static AllOfMatcher Animation {
            get {
                if (_matcherAnimation == null) {
                    _matcherAnimation = new Matcher(ComponentIds.Animation);
                }

                return _matcherAnimation;
            }
        }
    }
}
