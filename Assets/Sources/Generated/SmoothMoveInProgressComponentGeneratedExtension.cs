namespace Entitas {
    public partial class Entity {
        static readonly SmoothMoveInProgressComponent smoothMoveInProgressComponent = new SmoothMoveInProgressComponent();

        public bool isSmoothMoveInProgress {
            get { return HasComponent(ComponentIds.SmoothMoveInProgress); }
            set {
                if (value != isSmoothMoveInProgress) {
                    if (value) {
                        AddComponent(ComponentIds.SmoothMoveInProgress, smoothMoveInProgressComponent);
                    } else {
                        RemoveComponent(ComponentIds.SmoothMoveInProgress);
                    }
                }
            }
        }

        public Entity IsSmoothMoveInProgress(bool value) {
            isSmoothMoveInProgress = value;
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherSmoothMoveInProgress;

        public static AllOfMatcher SmoothMoveInProgress {
            get {
                if (_matcherSmoothMoveInProgress == null) {
                    _matcherSmoothMoveInProgress = new Matcher(ComponentIds.SmoothMoveInProgress);
                }

                return _matcherSmoothMoveInProgress;
            }
        }
    }
}
