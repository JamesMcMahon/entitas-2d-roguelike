namespace Entitas {
    public partial class Entity {
        static readonly GameOverComponent gameOverComponent = new GameOverComponent();

        public bool isGameOver {
            get { return HasComponent(ComponentIds.GameOver); }
            set {
                if (value != isGameOver) {
                    if (value) {
                        AddComponent(ComponentIds.GameOver, gameOverComponent);
                    } else {
                        RemoveComponent(ComponentIds.GameOver);
                    }
                }
            }
        }

        public Entity IsGameOver(bool value) {
            isGameOver = value;
            return this;
        }
    }

    public partial class Pool {
        public Entity gameOverEntity { get { return GetGroup(Matcher.GameOver).GetSingleEntity(); } }

        public bool isGameOver {
            get { return gameOverEntity != null; }
            set {
                var entity = gameOverEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isGameOver = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherGameOver;

        public static AllOfMatcher GameOver {
            get {
                if (_matcherGameOver == null) {
                    _matcherGameOver = new Matcher(ComponentIds.GameOver);
                }

                return _matcherGameOver;
            }
        }
    }
}
