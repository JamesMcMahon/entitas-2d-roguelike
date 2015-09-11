namespace Entitas {
    public partial class Pool {
        public ISystem CreateGameOverSystem() {
            return this.CreateSystem<GameOverSystem>();
        }
    }
}