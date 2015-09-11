namespace Entitas {
    public partial class Pool {
        public ISystem CreateDestructibleSystem() {
            return this.CreateSystem<DestructibleSystem>();
        }
    }
}