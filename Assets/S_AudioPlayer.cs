using FMODUnity;
using UnityEngine;

public class S_AudioPlayer : MonoBehaviour
{
    public bool playOnAwake = true;
    [SerializeField] EventReference audioToPlay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(playOnAwake)
            AudioManager.PlayAudio(audioToPlay);
    }

}
