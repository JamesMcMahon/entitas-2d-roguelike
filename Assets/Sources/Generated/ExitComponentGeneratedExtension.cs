namespace Entitas {
    public partial class Entity {
        static readonly ExitComponent exitComponent = new ExitComponent();

        public bool isExit {
            get { return HasComponent(ComponentIds.Exit); }
            set {
                if (value != isExit) {
                    if (value) {
                        AddComponent(ComponentIds.Exit, exitComponent);
                    } else {
                        RemoveComponent(ComponentIds.Exit);
                    }
                }
            }
        }

        public Entity IsExit(bool value) {
            isExit = value;
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherExit;

        public static AllOfMatcher Exit {
            get {
                if (_matcherExit == null) {
                    _matcherExit = new Matcher(ComponentIds.Exit);
                }

                return _matcherExit;
            }
        }
    }
}
