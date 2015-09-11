using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public AudioDeathSourceComponent audioDeathSource { get { return (AudioDeathSourceComponent)GetComponent(ComponentIds.AudioDeathSource); } }

        public bool hasAudioDeathSource { get { return HasComponent(ComponentIds.AudioDeathSource); } }

        static readonly Stack<AudioDeathSourceComponent> _audioDeathSourceComponentPool = new Stack<AudioDeathSourceComponent>();

        public static void ClearAudioDeathSourceComponentPool() {
            _audioDeathSourceComponentPool.Clear();
        }

        public Entity AddAudioDeathSource(Audio[] newClips, bool newRandomizePitch) {
            var component = _audioDeathSourceComponentPool.Count > 0 ? _audioDeathSourceComponentPool.Pop() : new AudioDeathSourceComponent();
            component.clips = newClips;
            component.randomizePitch = newRandomizePitch;
            return AddComponent(ComponentIds.AudioDeathSource, component);
        }

        public Entity ReplaceAudioDeathSource(Audio[] newClips, bool newRandomizePitch) {
            var previousComponent = hasAudioDeathSource ? audioDeathSource : null;
            var component = _audioDeathSourceComponentPool.Count > 0 ? _audioDeathSourceComponentPool.Pop() : new AudioDeathSourceComponent();
            component.clips = newClips;
            component.randomizePitch = newRandomizePitch;
            ReplaceComponent(ComponentIds.AudioDeathSource, component);
            if (previousComponent != null) {
                _audioDeathSourceComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveAudioDeathSource() {
            var component = audioDeathSource;
            RemoveComponent(ComponentIds.AudioDeathSource);
            _audioDeathSourceComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherAudioDeathSource;

        public static AllOfMatcher AudioDeathSource {
            get {
                if (_matcherAudioDeathSource == null) {
                    _matcherAudioDeathSource = new Matcher(ComponentIds.AudioDeathSource);
                }

                return _matcherAudioDeathSource;
            }
        }
    }
}
