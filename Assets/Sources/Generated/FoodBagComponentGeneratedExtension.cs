using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public FoodBagComponent foodBag { get { return (FoodBagComponent)GetComponent(ComponentIds.FoodBag); } }

        public bool hasFoodBag { get { return HasComponent(ComponentIds.FoodBag); } }

        static readonly Stack<FoodBagComponent> _foodBagComponentPool = new Stack<FoodBagComponent>();

        public static void ClearFoodBagComponentPool() {
            _foodBagComponentPool.Clear();
        }

        public Entity AddFoodBag(int newPoints) {
            var component = _foodBagComponentPool.Count > 0 ? _foodBagComponentPool.Pop() : new FoodBagComponent();
            component.points = newPoints;
            return AddComponent(ComponentIds.FoodBag, component);
        }

        public Entity ReplaceFoodBag(int newPoints) {
            var previousComponent = hasFoodBag ? foodBag : null;
            var component = _foodBagComponentPool.Count > 0 ? _foodBagComponentPool.Pop() : new FoodBagComponent();
            component.points = newPoints;
            ReplaceComponent(ComponentIds.FoodBag, component);
            if (previousComponent != null) {
                _foodBagComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveFoodBag() {
            var component = foodBag;
            RemoveComponent(ComponentIds.FoodBag);
            _foodBagComponentPool.Push(component);
            return this;
        }
    }

    public partial class Pool {
        public Entity foodBagEntity { get { return GetGroup(Matcher.FoodBag).GetSingleEntity(); } }

        public FoodBagComponent foodBag { get { return foodBagEntity.foodBag; } }

        public bool hasFoodBag { get { return foodBagEntity != null; } }

        public Entity SetFoodBag(int newPoints) {
            if (hasFoodBag) {
                throw new SingleEntityException(Matcher.FoodBag);
            }
            var entity = CreateEntity();
            entity.AddFoodBag(newPoints);
            return entity;
        }

        public Entity ReplaceFoodBag(int newPoints) {
            var entity = foodBagEntity;
            if (entity == null) {
                entity = SetFoodBag(newPoints);
            } else {
                entity.ReplaceFoodBag(newPoints);
            }

            return entity;
        }

        public void RemoveFoodBag() {
            DestroyEntity(foodBagEntity);
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherFoodBag;

        public static AllOfMatcher FoodBag {
            get {
                if (_matcherFoodBag == null) {
                    _matcherFoodBag = new Matcher(ComponentIds.FoodBag);
                }

                return _matcherFoodBag;
            }
        }
    }
}
