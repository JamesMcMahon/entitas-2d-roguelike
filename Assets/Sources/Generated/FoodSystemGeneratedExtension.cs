namespace Entitas {
    public partial class Pool {
        public ISystem CreateFoodSystem() {
            return this.CreateSystem<FoodSystem>();
        }
    }
}