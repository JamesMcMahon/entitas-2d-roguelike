using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public AudioPickupSourceComponent audioPickupSource { get { return (AudioPickupSourceComponent)GetComponent(ComponentIds.AudioPickupSource); } }

        public bool hasAudioPickupSource { get { return HasComponent(ComponentIds.AudioPickupSource); } }

        static readonly Stack<AudioPickupSourceComponent> _audioPickupSourceComponentPool = new Stack<AudioPickupSourceComponent>();

        public static void ClearAudioPickupSourceComponentPool() {
            _audioPickupSourceComponentPool.Clear();
        }

        public Entity AddAudioPickupSource(Audio[] newClips, bool newRandomizePitch) {
            var component = _audioPickupSourceComponentPool.Count > 0 ? _audioPickupSourceComponentPool.Pop() : new AudioPickupSourceComponent();
            component.clips = newClips;
            component.randomizePitch = newRandomizePitch;
            return AddComponent(ComponentIds.AudioPickupSource, component);
        }

        public Entity ReplaceAudioPickupSource(Audio[] newClips, bool newRandomizePitch) {
            var previousComponent = hasAudioPickupSource ? audioPickupSource : null;
            var component = _audioPickupSourceComponentPool.Count > 0 ? _audioPickupSourceComponentPool.Pop() : new AudioPickupSourceComponent();
            component.clips = newClips;
            component.randomizePitch = newRandomizePitch;
            ReplaceComponent(ComponentIds.AudioPickupSource, component);
            if (previousComponent != null) {
                _audioPickupSourceComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveAudioPickupSource() {
            var component = audioPickupSource;
            RemoveComponent(ComponentIds.AudioPickupSource);
            _audioPickupSourceComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherAudioPickupSource;

        public static AllOfMatcher AudioPickupSource {
            get {
                if (_matcherAudioPickupSource == null) {
                    _matcherAudioPickupSource = new Matcher(ComponentIds.AudioPickupSource);
                }

                return _matcherAudioPickupSource;
            }
        }
    }
}
