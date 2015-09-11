namespace Entitas {
    public partial class Pool {
        public ISystem CreateSmoothMoveSystem() {
            return this.CreateSystem<SmoothMoveSystem>();
        }
    }
}