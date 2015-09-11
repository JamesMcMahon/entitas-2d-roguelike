using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public FoodDamagerComponent foodDamager { get { return (FoodDamagerComponent)GetComponent(ComponentIds.FoodDamager); } }

        public bool hasFoodDamager { get { return HasComponent(ComponentIds.FoodDamager); } }

        static readonly Stack<FoodDamagerComponent> _foodDamagerComponentPool = new Stack<FoodDamagerComponent>();

        public static void ClearFoodDamagerComponentPool() {
            _foodDamagerComponentPool.Clear();
        }

        public Entity AddFoodDamager(int newPoints) {
            var component = _foodDamagerComponentPool.Count > 0 ? _foodDamagerComponentPool.Pop() : new FoodDamagerComponent();
            component.points = newPoints;
            return AddComponent(ComponentIds.FoodDamager, component);
        }

        public Entity ReplaceFoodDamager(int newPoints) {
            var previousComponent = hasFoodDamager ? foodDamager : null;
            var component = _foodDamagerComponentPool.Count > 0 ? _foodDamagerComponentPool.Pop() : new FoodDamagerComponent();
            component.points = newPoints;
            ReplaceComponent(ComponentIds.FoodDamager, component);
            if (previousComponent != null) {
                _foodDamagerComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveFoodDamager() {
            var component = foodDamager;
            RemoveComponent(ComponentIds.FoodDamager);
            _foodDamagerComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherFoodDamager;

        public static AllOfMatcher FoodDamager {
            get {
                if (_matcherFoodDamager == null) {
                    _matcherFoodDamager = new Matcher(ComponentIds.FoodDamager);
                }

                return _matcherFoodDamager;
            }
        }
    }
}
