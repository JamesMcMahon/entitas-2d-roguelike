namespace Entitas {
    public partial class Pool {
        public ISystem CreateGameBoardCacheSystem() {
            return this.CreateSystem<GameBoardCacheSystem>();
        }
    }
}