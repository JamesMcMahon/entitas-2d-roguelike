namespace Entitas {
    public partial class Pool {
        public ISystem CreateGameStartSystem() {
            return this.CreateSystem<GameStartSystem>();
        }
    }
}