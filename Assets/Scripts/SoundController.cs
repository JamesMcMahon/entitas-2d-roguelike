using Entitas;
using IListExtensions;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    void Start()
    {
        Pools.pool.GetGroup(Matcher.Audio).OnEntityAdded += OnAudioAdded;
        Pools.pool.GetGroup(Matcher.GameOver).OnEntityAdded += (group, entity, index, component) => StopMusic();
    }

    void OnAudioAdded(Group group, Entity entity, int index, IComponent component)
    {
        var audioComponent = (AudioComponent)component;
        var audioName = audioComponent.clipNames.Random();
        var audioClip = Resources.Load<AudioClip>("Audio/" + audioName);
        
        if (audioClip != null)
        {
            Play(audioClip, audioComponent.randomizePitch);
        }

        // only play once
        Pools.pool.DestroyEntity(entity);
    }

    void Play(AudioClip clip, bool randomize = false)
    {
        efxSource.clip = clip;

        if (!randomize)
        {
            efxSource.Play();
            return;
        }

        var originalPitch = efxSource.pitch;
        efxSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.Play();
        efxSource.pitch = originalPitch;
        return;
    }

    void StopMusic()
    {
        musicSource.Stop();
    }
}
