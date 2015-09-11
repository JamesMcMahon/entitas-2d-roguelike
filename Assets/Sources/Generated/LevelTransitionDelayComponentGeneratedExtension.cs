namespace Entitas {
    public partial class Entity {
        static readonly LevelTransitionDelayComponent levelTransitionDelayComponent = new LevelTransitionDelayComponent();

        public bool isLevelTransitionDelay {
            get { return HasComponent(ComponentIds.LevelTransitionDelay); }
            set {
                if (value != isLevelTransitionDelay) {
                    if (value) {
                        AddComponent(ComponentIds.LevelTransitionDelay, levelTransitionDelayComponent);
                    } else {
                        RemoveComponent(ComponentIds.LevelTransitionDelay);
                    }
                }
            }
        }

        public Entity IsLevelTransitionDelay(bool value) {
            isLevelTransitionDelay = value;
            return this;
        }
    }

    public partial class Pool {
        public Entity levelTransitionDelayEntity { get { return GetGroup(Matcher.LevelTransitionDelay).GetSingleEntity(); } }

        public bool isLevelTransitionDelay {
            get { return levelTransitionDelayEntity != null; }
            set {
                var entity = levelTransitionDelayEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isLevelTransitionDelay = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherLevelTransitionDelay;

        public static AllOfMatcher LevelTransitionDelay {
            get {
                if (_matcherLevelTransitionDelay == null) {
                    _matcherLevelTransitionDelay = new Matcher(ComponentIds.LevelTransitionDelay);
                }

                return _matcherLevelTransitionDelay;
            }
        }
    }
}
