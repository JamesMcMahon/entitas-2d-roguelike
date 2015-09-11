using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public NestedViewComponent nestedView { get { return (NestedViewComponent)GetComponent(ComponentIds.NestedView); } }

        public bool hasNestedView { get { return HasComponent(ComponentIds.NestedView); } }

        static readonly Stack<NestedViewComponent> _nestedViewComponentPool = new Stack<NestedViewComponent>();

        public static void ClearNestedViewComponentPool() {
            _nestedViewComponentPool.Clear();
        }

        public Entity AddNestedView(string newName) {
            var component = _nestedViewComponentPool.Count > 0 ? _nestedViewComponentPool.Pop() : new NestedViewComponent();
            component.name = newName;
            return AddComponent(ComponentIds.NestedView, component);
        }

        public Entity ReplaceNestedView(string newName) {
            var previousComponent = hasNestedView ? nestedView : null;
            var component = _nestedViewComponentPool.Count > 0 ? _nestedViewComponentPool.Pop() : new NestedViewComponent();
            component.name = newName;
            ReplaceComponent(ComponentIds.NestedView, component);
            if (previousComponent != null) {
                _nestedViewComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveNestedView() {
            var component = nestedView;
            RemoveComponent(ComponentIds.NestedView);
            _nestedViewComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherNestedView;

        public static AllOfMatcher NestedView {
            get {
                if (_matcherNestedView == null) {
                    _matcherNestedView = new Matcher(ComponentIds.NestedView);
                }

                return _matcherNestedView;
            }
        }
    }
}
