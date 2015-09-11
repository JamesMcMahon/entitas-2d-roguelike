namespace Entitas {
    public partial class Pool {
        public ISystem CreateAIMoveSystem() {
            return this.CreateSystem<AIMoveSystem>();
        }
    }
}