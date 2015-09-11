using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public AudioAttackSourceComponent audioAttackSource { get { return (AudioAttackSourceComponent)GetComponent(ComponentIds.AudioAttackSource); } }

        public bool hasAudioAttackSource { get { return HasComponent(ComponentIds.AudioAttackSource); } }

        static readonly Stack<AudioAttackSourceComponent> _audioAttackSourceComponentPool = new Stack<AudioAttackSourceComponent>();

        public static void ClearAudioAttackSourceComponentPool() {
            _audioAttackSourceComponentPool.Clear();
        }

        public Entity AddAudioAttackSource(Audio[] newClips, bool newRandomizePitch) {
            var component = _audioAttackSourceComponentPool.Count > 0 ? _audioAttackSourceComponentPool.Pop() : new AudioAttackSourceComponent();
            component.clips = newClips;
            component.randomizePitch = newRandomizePitch;
            return AddComponent(ComponentIds.AudioAttackSource, component);
        }

        public Entity ReplaceAudioAttackSource(Audio[] newClips, bool newRandomizePitch) {
            var previousComponent = hasAudioAttackSource ? audioAttackSource : null;
            var component = _audioAttackSourceComponentPool.Count > 0 ? _audioAttackSourceComponentPool.Pop() : new AudioAttackSourceComponent();
            component.clips = newClips;
            component.randomizePitch = newRandomizePitch;
            ReplaceComponent(ComponentIds.AudioAttackSource, component);
            if (previousComponent != null) {
                _audioAttackSourceComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveAudioAttackSource() {
            var component = audioAttackSource;
            RemoveComponent(ComponentIds.AudioAttackSource);
            _audioAttackSourceComponentPool.Push(component);
            return this;
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherAudioAttackSource;

        public static AllOfMatcher AudioAttackSource {
            get {
                if (_matcherAudioAttackSource == null) {
                    _matcherAudioAttackSource = new Matcher(ComponentIds.AudioAttackSource);
                }

                return _matcherAudioAttackSource;
            }
        }
    }
}
