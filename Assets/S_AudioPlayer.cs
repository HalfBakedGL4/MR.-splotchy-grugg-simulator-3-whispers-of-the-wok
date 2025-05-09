using FMODUnity;
using UnityEngine;

public class S_AudioPlayer : MonoBehaviour
{
    public bool playOnAwake = true;
    [SerializeField] AudioReference audioToPlay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        while(true)
        {
            if (playOnAwake)
                await audioToPlay.PlayAsync();
        }
    }

    public void PlayAudio()
    {
        audioToPlay.Play();
    }

}
