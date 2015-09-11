using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public ResourceComponent resource { get { return (ResourceComponent)GetComponent(ComponentIds.Resource); } }

        public bool hasResource { get { return HasComponent(ComponentIds.Resource); } }

        static readonly Stack<ResourceComponent> _resourceComponentPool = new Stack<ResourceComponent>();

        public static void ClearResourceComponentPool() {
            _resourceComponentPool.Clear();
        }

        public Entity AddResource(Prefab newPrefab) {
            var component = _resourceComponentPool.Count > 0 ? _resourceComponentPool.Pop() : new ResourceComponent();
            component.prefab = newPrefab;
            return AddComponent(ComponentIds.Resource, component);
        }

        public Entity ReplaceResource(Prefab newPrefab) {
            var previousComponent = hasResource ? resource : null;
            var component = _resourceComponentPool.Count > 0 ? _resourceComponentPool.Pop() : new ResourceComponent();
            component.prefab = newPrefab;
            ReplaceComponent(ComponentIds.Resource, component);
            if (previousComponent != null) {
                _resourceComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveResource() {
            var component = resource;
            RemoveComponent(ComponentIds.Resource);
            _resourceComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherResource;

        public static AllOfMatcher Resource {
            get {
                if (_matcherResource == null) {
                    _matcherResource = new Matcher(ComponentIds.Resource);
                }

                return _matcherResource;
            }
        }
    }
}
