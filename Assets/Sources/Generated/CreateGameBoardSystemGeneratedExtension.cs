namespace Entitas {
    public partial class Pool {
        public ISystem CreateCreateGameBoardSystem() {
            return this.CreateSystem<CreateGameBoardSystem>();
        }
    }
}