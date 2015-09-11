namespace Entitas {
    public partial class Pool {
        public ISystem CreateTurnSystem() {
            return this.CreateSystem<TurnSystem>();
        }
    }
}