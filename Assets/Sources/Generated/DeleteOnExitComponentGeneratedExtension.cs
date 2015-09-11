namespace Entitas {
    public partial class Entity {
        static readonly DeleteOnExitComponent deleteOnExitComponent = new DeleteOnExitComponent();

        public bool isDeleteOnExit {
            get { return HasComponent(ComponentIds.DeleteOnExit); }
            set {
                if (value != isDeleteOnExit) {
                    if (value) {
                        AddComponent(ComponentIds.DeleteOnExit, deleteOnExitComponent);
                    } else {
                        RemoveComponent(ComponentIds.DeleteOnExit);
                    }
                }
            }
        }

        public Entity IsDeleteOnExit(bool value) {
            isDeleteOnExit = value;
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherDeleteOnExit;

        public static AllOfMatcher DeleteOnExit {
            get {
                if (_matcherDeleteOnExit == null) {
                    _matcherDeleteOnExit = new Matcher(ComponentIds.DeleteOnExit);
                }

                return _matcherDeleteOnExit;
            }
        }
    }
}
