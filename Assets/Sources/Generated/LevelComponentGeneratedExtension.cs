using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public LevelComponent level { get { return (LevelComponent)GetComponent(ComponentIds.Level); } }

        public bool hasLevel { get { return HasComponent(ComponentIds.Level); } }

        static readonly Stack<LevelComponent> _levelComponentPool = new Stack<LevelComponent>();

        public static void ClearLevelComponentPool() {
            _levelComponentPool.Clear();
        }

        public Entity AddLevel(int newLevel) {
            var component = _levelComponentPool.Count > 0 ? _levelComponentPool.Pop() : new LevelComponent();
            component.level = newLevel;
            return AddComponent(ComponentIds.Level, component);
        }

        public Entity ReplaceLevel(int newLevel) {
            var previousComponent = hasLevel ? level : null;
            var component = _levelComponentPool.Count > 0 ? _levelComponentPool.Pop() : new LevelComponent();
            component.level = newLevel;
            ReplaceComponent(ComponentIds.Level, component);
            if (previousComponent != null) {
                _levelComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveLevel() {
            var component = level;
            RemoveComponent(ComponentIds.Level);
            _levelComponentPool.Push(component);
            return this;
        }
    }

    public partial class Pool {
        public Entity levelEntity { get { return GetGroup(Matcher.Level).GetSingleEntity(); } }

        public LevelComponent level { get { return levelEntity.level; } }

        public bool hasLevel { get { return levelEntity != null; } }

        public Entity SetLevel(int newLevel) {
            if (hasLevel) {
                throw new SingleEntityException(Matcher.Level);
            }
            var entity = CreateEntity();
            entity.AddLevel(newLevel);
            return entity;
        }

        public Entity ReplaceLevel(int newLevel) {
            var entity = levelEntity;
            if (entity == null) {
                entity = SetLevel(newLevel);
            } else {
                entity.ReplaceLevel(newLevel);
            }

            return entity;
        }

        public void RemoveLevel() {
            DestroyEntity(levelEntity);
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherLevel;

        public static AllOfMatcher Level {
            get {
                if (_matcherLevel == null) {
                    _matcherLevel = new Matcher(ComponentIds.Level);
                }

                return _matcherLevel;
            }
        }
    }
}
