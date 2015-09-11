namespace Entitas {
    public partial class Entity {
        static readonly ActiveTurnBasedComponent activeTurnBasedComponent = new ActiveTurnBasedComponent();

        public bool isActiveTurnBased {
            get { return HasComponent(ComponentIds.ActiveTurnBased); }
            set {
                if (value != isActiveTurnBased) {
                    if (value) {
                        AddComponent(ComponentIds.ActiveTurnBased, activeTurnBasedComponent);
                    } else {
                        RemoveComponent(ComponentIds.ActiveTurnBased);
                    }
                }
            }
        }

        public Entity IsActiveTurnBased(bool value) {
            isActiveTurnBased = value;
            return this;
        }
    }

    public partial class Pool {
        public Entity activeTurnBasedEntity { get { return GetGroup(Matcher.ActiveTurnBased).GetSingleEntity(); } }

        public bool isActiveTurnBased {
            get { return activeTurnBasedEntity != null; }
            set {
                var entity = activeTurnBasedEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isActiveTurnBased = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherActiveTurnBased;

        public static AllOfMatcher ActiveTurnBased {
            get {
                if (_matcherActiveTurnBased == null) {
                    _matcherActiveTurnBased = new Matcher(ComponentIds.ActiveTurnBased);
                }

                return _matcherActiveTurnBased;
            }
        }
    }
}
