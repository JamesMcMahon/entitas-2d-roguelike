namespace Entitas {
    public partial class Entity {
        static readonly AIMoveComponent aIMoveComponent = new AIMoveComponent();

        public bool isAIMove {
            get { return HasComponent(ComponentIds.AIMove); }
            set {
                if (value != isAIMove) {
                    if (value) {
                        AddComponent(ComponentIds.AIMove, aIMoveComponent);
                    } else {
                        RemoveComponent(ComponentIds.AIMove);
                    }
                }
            }
        }

        public Entity IsAIMove(bool value) {
            isAIMove = value;
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherAIMove;

        public static AllOfMatcher AIMove {
            get {
                if (_matcherAIMove == null) {
                    _matcherAIMove = new Matcher(ComponentIds.AIMove);
                }

                return _matcherAIMove;
            }
        }
    }
}
