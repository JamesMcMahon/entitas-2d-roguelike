namespace Entitas {
    public partial class Entity {
        static readonly SkipTurnComponent skipTurnComponent = new SkipTurnComponent();

        public bool isSkipTurn {
            get { return HasComponent(ComponentIds.SkipTurn); }
            set {
                if (value != isSkipTurn) {
                    if (value) {
                        AddComponent(ComponentIds.SkipTurn, skipTurnComponent);
                    } else {
                        RemoveComponent(ComponentIds.SkipTurn);
                    }
                }
            }
        }

        public Entity IsSkipTurn(bool value) {
            isSkipTurn = value;
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherSkipTurn;

        public static AllOfMatcher SkipTurn {
            get {
                if (_matcherSkipTurn == null) {
                    _matcherSkipTurn = new Matcher(ComponentIds.SkipTurn);
                }

                return _matcherSkipTurn;
            }
        }
    }
}
