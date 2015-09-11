namespace Entitas {
    public partial class Pool {
        public ISystem CreateAnimationSystem() {
            return this.CreateSystem<AnimationSystem>();
        }
    }
}