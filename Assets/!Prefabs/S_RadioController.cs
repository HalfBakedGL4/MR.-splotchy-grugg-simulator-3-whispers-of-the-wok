using Fusion;
using UnityEngine;

public class S_RadioController : NetworkBehaviour
{
    [SerializeField] FMODUnity.StudioEventEmitter emitter;

    public void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.SetParameter("HighEQ", 0.7f);
        emitter.SetParameter("MidEQ", 0.7f);
        emitter.SetParameter("LowEQ", 0.7f);
        emitter.Play();
    }


    public void SetVolume(float volume)
    {
        emitter.EventInstance.setParameterByName("Volume", Mathf.Clamp(volume, 0.1f, 1));
        
    }

    public void SetChannel(float channel)
    {
        emitter.EventInstance.setParameterByName("Radio", channel);
        Debug.Log("Set radio channel to: " + channel);
    }
}
