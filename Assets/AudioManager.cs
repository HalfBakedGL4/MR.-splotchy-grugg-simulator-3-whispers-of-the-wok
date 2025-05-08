using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public EventReference bgAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.PlayAudio(bgAudio, true);
    }

    public static async void PlayAudio(EventReference audio, bool repeating = false, GameObject attachTo = null)
    {
        EventInstance instance = RuntimeManager.CreateInstance(audio);

        if(attachTo != null)
        {
            if (attachTo.TryGetComponent(out Rigidbody rb))
            {
                RuntimeManager.AttachInstanceToGameObject(instance, attachTo, rb);
            } else
            {
                RuntimeManager.AttachInstanceToGameObject(instance, attachTo);
            }
        }

        instance.start();
        await Task.Delay(1);
    }
}
