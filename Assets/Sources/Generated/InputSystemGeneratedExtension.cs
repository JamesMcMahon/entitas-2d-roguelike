namespace Entitas {
    public partial class Pool {
        public ISystem CreateInputSystem() {
            return this.CreateSystem<InputSystem>();
        }
    }
}