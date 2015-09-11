namespace Entitas {
    public partial class Entity {
        static readonly ControllableComponent controllableComponent = new ControllableComponent();

        public bool isControllable {
            get { return HasComponent(ComponentIds.Controllable); }
            set {
                if (value != isControllable) {
                    if (value) {
                        AddComponent(ComponentIds.Controllable, controllableComponent);
                    } else {
                        RemoveComponent(ComponentIds.Controllable);
                    }
                }
            }
        }

        public Entity IsControllable(bool value) {
            isControllable = value;
            return this;
        }
    }

    public partial class Pool {
        public Entity controllableEntity { get { return GetGroup(Matcher.Controllable).GetSingleEntity(); } }

        public bool isControllable {
            get { return controllableEntity != null; }
            set {
                var entity = controllableEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isControllable = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherControllable;

        public static AllOfMatcher Controllable {
            get {
                if (_matcherControllable == null) {
                    _matcherControllable = new Matcher(ComponentIds.Controllable);
                }

                return _matcherControllable;
            }
        }
    }
}
