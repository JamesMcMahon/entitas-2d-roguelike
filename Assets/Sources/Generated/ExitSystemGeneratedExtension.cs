namespace Entitas {
    public partial class Pool {
        public ISystem CreateExitSystem() {
            return this.CreateSystem<ExitSystem>();
        }
    }
}