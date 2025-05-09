using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;
using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Events;
using static AudioManager;

public enum AudioCategory
{
    None = 0,
    SFX = 1,
    Music = 2
}

/// <summary>
/// a reference to audio
/// </summary>
[Serializable]
public struct AudioReference
{
    public EventReference audio;
    public GameObject emitter;

    [Space]

    public AudioCategory category;
    public Volume volume;

    float currentVolume
    {
        get
        {
            switch(category)
            {
                case AudioCategory.SFX:
                    {
                        return audioManager.MasterVolume.value * volume.value * audioManager.SFXVolume.value;
                    }
                case AudioCategory.Music:
                    {
                        return audioManager.MasterVolume.value * volume.value * audioManager.MusicVolume.value;
                    }
                default:
                    {
                        return audioManager.MasterVolume.value * volume.value;
                    }
            }
        }
    }

    public AudioReference(EventReference reference, GameObject emitter, AudioCategory category, Volume volume)
    {
        audio = reference;
        this.emitter = emitter;
        this.category = category;
        this.volume = volume;
    }

    /// <summary>
    /// Play one instance of audio
    /// </summary>
    /// <returns>Instance of the played audio</returns>
    public EventInstance Play()
    {
        EventInstance audioInstance = RuntimeManager.CreateInstance(audio);
        audioInstance.setVolume(currentVolume);

        if (emitter != null)
        {
            if (emitter.TryGetComponent(out Rigidbody rb))
            {
                RuntimeManager.AttachInstanceToGameObject(audioInstance, emitter, rb);
            }
            else
            {
                RuntimeManager.AttachInstanceToGameObject(audioInstance, emitter);
            }
        }

        audioInstance.start();
        return audioInstance;
    }
    /// <summary>
    /// Play one instance of audio
    /// </summary>
    /// <param name="audioInstance">out of the audio instance</param>
    /// <returns>Instance of the played audio</returns>
    public EventInstance Play(out EventInstance audioInstance)
    {
        audioInstance = Play();
        return audioInstance;
    }
    /// <summary>
    /// Play one instance of audio, can delay until after the audio finished playing
    /// </summary>
    /// <param name="additionalMilliseconds">additional milliseconds to wait</param>
    /// <returns>Instance of the played audio</returns>
    public async Task<EventInstance> PlayAsync(int additionalMilliseconds = 0)
    {
        Play(out EventInstance audioInstance);

        audioInstance.getDescription(out EventDescription description);
        description.getLength(out int audioLength);

        await Task.Delay(audioLength + additionalMilliseconds);
        return audioInstance;
    }
}

[Serializable]
public class Volume
{
    [field: SerializeField, Range(0, 1)] public float value { get; private set; }

    public UnityEvent OnUpdated;

    public void UpdateVolume(float amount)
    {
        value += amount;
    }

    public Volume(float value)
    {
        this.value = value;
    }
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;

    [field: SerializeField] public Volume MasterVolume { get; private set; } = new(1);
    [field: SerializeField] public Volume SFXVolume { get; private set; } = new(1);
    [field: SerializeField] public Volume MusicVolume { get; private set; } = new(1);

    private void Start()
    {
        audioManager = this;
        DontDestroyOnLoad(gameObject);
    }
}
