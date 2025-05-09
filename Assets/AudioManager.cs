using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public static class AudioManager
{
    /// <summary>
    /// Play one instance of audio
    /// </summary>
    /// <param name="audio">Refrence to the audio clip</param>
    /// <param name="attachTo">Object to attach to</param>
    /// <returns>Instance of the played audio</returns>
    public static EventInstance PlayAudio(EventReference audio, GameObject attachTo = null)
    {
        EventInstance audioInstance = RuntimeManager.CreateInstance(audio);

        if (attachTo != null)
        {
            if (attachTo.TryGetComponent(out Rigidbody rb))
            {
                RuntimeManager.AttachInstanceToGameObject(audioInstance, attachTo, rb);
            }
            else
            {
                RuntimeManager.AttachInstanceToGameObject(audioInstance, attachTo);
            }
        }

        audioInstance.start();
        return audioInstance;
    }
    /// <summary>
    /// Play one instance of audio
    /// </summary>
    /// <param name="audio">Refrence to the audio clip</param>
    /// <param name="audioInstance">out of the audio instance</param>
    /// <param name="attachTo">Object to attach to</param>
    /// <returns>Instance of the played audio</returns>
    public static EventInstance PlayAudio(EventReference audio, out EventInstance audioInstance, GameObject attachTo = null)
    {
        audioInstance = PlayAudio(audio, attachTo);
        return audioInstance;
    }
    /// <summary>
    /// Play one instance of audio, can delay until after the audio finished playing
    /// </summary>
    /// <param name="audio">Refrence to the audio clip</param>
    /// <param name="additionalMilliseconds">additional milliseconds to wait</param>
    /// <param name="attachTo">Object to attach to</param>
    /// <returns>Instance of the played audio</returns>
    public static async Task<EventInstance> AsyncPlayAudio(EventReference audio, int additionalMilliseconds = 0, GameObject attachTo = null)
    {
        PlayAudio(audio, out EventInstance audioInstance, attachTo);

        audioInstance.getDescription(out EventDescription description);
        description.getLength(out int audioLength);

        await Task.Delay(audioLength + additionalMilliseconds);
        return audioInstance;
    }
    /// <summary>
    /// Repeat audio, can delay until after all instances have played
    /// </summary>
    /// <param name="audio">Refrence to the audio clip</param>
    /// <param name="repeatTimes">How many times to play, leave at 0 to repeat forever</param>
    /// <param name="additionalDelay">Delay between each instance</param>
    /// <param name="attachTo">Object to attach to</param>
    public static async Task AsyncPlayAudioRepeating(EventReference audio, int repeatTimes, int additionalDelay = 0, GameObject attachTo = null)
    {
        if(repeatTimes == 1)
        {
            PlayAudio(audio, out EventInstance instance, attachTo);
            return;
        }

        if(repeatTimes <= 0)
        {
            while(true)
            {
                await AsyncPlayAudio(audio, additionalDelay, attachTo);
            }

        } else
        {
            for (int i = 0; i < repeatTimes; i++)
            {
                await AsyncPlayAudio(audio, additionalDelay, attachTo);
            }
        }
    }
}
