namespace Entitas {
    public partial class Pool {
        public ISystem CreateDamageSpriteSystem() {
            return this.CreateSystem<DamageSpriteSystem>();
        }
    }
}