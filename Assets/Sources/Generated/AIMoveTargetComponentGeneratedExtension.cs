namespace Entitas {
    public partial class Entity {
        static readonly AIMoveTargetComponent aIMoveTargetComponent = new AIMoveTargetComponent();

        public bool isAIMoveTarget {
            get { return HasComponent(ComponentIds.AIMoveTarget); }
            set {
                if (value != isAIMoveTarget) {
                    if (value) {
                        AddComponent(ComponentIds.AIMoveTarget, aIMoveTargetComponent);
                    } else {
                        RemoveComponent(ComponentIds.AIMoveTarget);
                    }
                }
            }
        }

        public Entity IsAIMoveTarget(bool value) {
            isAIMoveTarget = value;
            return this;
        }
    }

    public partial class Pool {
        public Entity aIMoveTargetEntity { get { return GetGroup(Matcher.AIMoveTarget).GetSingleEntity(); } }

        public bool isAIMoveTarget {
            get { return aIMoveTargetEntity != null; }
            set {
                var entity = aIMoveTargetEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isAIMoveTarget = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherAIMoveTarget;

        public static AllOfMatcher AIMoveTarget {
            get {
                if (_matcherAIMoveTarget == null) {
                    _matcherAIMoveTarget = new Matcher(ComponentIds.AIMoveTarget);
                }

                return _matcherAIMoveTarget;
            }
        }
    }
}
