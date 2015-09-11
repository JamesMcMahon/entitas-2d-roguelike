using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public FoodComponent food { get { return (FoodComponent)GetComponent(ComponentIds.Food); } }

        public bool hasFood { get { return HasComponent(ComponentIds.Food); } }

        static readonly Stack<FoodComponent> _foodComponentPool = new Stack<FoodComponent>();

        public static void ClearFoodComponentPool() {
            _foodComponentPool.Clear();
        }

        public Entity AddFood(int newPoints) {
            var component = _foodComponentPool.Count > 0 ? _foodComponentPool.Pop() : new FoodComponent();
            component.points = newPoints;
            return AddComponent(ComponentIds.Food, component);
        }

        public Entity ReplaceFood(int newPoints) {
            var previousComponent = hasFood ? food : null;
            var component = _foodComponentPool.Count > 0 ? _foodComponentPool.Pop() : new FoodComponent();
            component.points = newPoints;
            ReplaceComponent(ComponentIds.Food, component);
            if (previousComponent != null) {
                _foodComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveFood() {
            var component = food;
            RemoveComponent(ComponentIds.Food);
            _foodComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherFood;

        public static AllOfMatcher Food {
            get {
                if (_matcherFood == null) {
                    _matcherFood = new Matcher(ComponentIds.Food);
                }

                return _matcherFood;
            }
        }
    }
}
