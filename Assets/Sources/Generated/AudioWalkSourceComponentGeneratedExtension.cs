using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public AudioWalkSourceComponent audioWalkSource { get { return (AudioWalkSourceComponent)GetComponent(ComponentIds.AudioWalkSource); } }

        public bool hasAudioWalkSource { get { return HasComponent(ComponentIds.AudioWalkSource); } }

        static readonly Stack<AudioWalkSourceComponent> _audioWalkSourceComponentPool = new Stack<AudioWalkSourceComponent>();

        public static void ClearAudioWalkSourceComponentPool() {
            _audioWalkSourceComponentPool.Clear();
        }

        public Entity AddAudioWalkSource(Audio[] newClips, bool newRandomizePitch) {
            var component = _audioWalkSourceComponentPool.Count > 0 ? _audioWalkSourceComponentPool.Pop() : new AudioWalkSourceComponent();
            component.clips = newClips;
            component.randomizePitch = newRandomizePitch;
            return AddComponent(ComponentIds.AudioWalkSource, component);
        }

        public Entity ReplaceAudioWalkSource(Audio[] newClips, bool newRandomizePitch) {
            var previousComponent = hasAudioWalkSource ? audioWalkSource : null;
            var component = _audioWalkSourceComponentPool.Count > 0 ? _audioWalkSourceComponentPool.Pop() : new AudioWalkSourceComponent();
            component.clips = newClips;
            component.randomizePitch = newRandomizePitch;
            ReplaceComponent(ComponentIds.AudioWalkSource, component);
            if (previousComponent != null) {
                _audioWalkSourceComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveAudioWalkSource() {
            var component = audioWalkSource;
            RemoveComponent(ComponentIds.AudioWalkSource);
            _audioWalkSourceComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherAudioWalkSource;

        public static AllOfMatcher AudioWalkSource {
            get {
                if (_matcherAudioWalkSource == null) {
                    _matcherAudioWalkSource = new Matcher(ComponentIds.AudioWalkSource);
                }

                return _matcherAudioWalkSource;
            }
        }
    }
}
