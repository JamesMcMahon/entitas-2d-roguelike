using Entitas;
using IListExtensions;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    PoolContext pool;

    void Start()
    {
        pool = Contexts.sharedInstance.pool;

        pool.GetGroup(PoolMatcher.Audio).OnEntityAdded += OnAudioAdded;
        pool.GetGroup(PoolMatcher.GameOver).OnEntityAdded += (group, entity, index, component) => StopMusic();
    }

    void OnAudioAdded(IGroup<PoolEntity> group, PoolEntity entity, int index,
                      IComponent component)
    {
        var audioComponent = (AudioComponent)component;
        var audioName = audioComponent.clipNames.Random();
        var audioClip = Resources.Load<AudioClip>("Audio/" + audioName);
        
        if (audioClip != null)
        {
            Play(audioClip, audioComponent.randomizePitch);
        }

        // only play once
        pool.DestroyEntity(entity);
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
