using Fusion;
using UnityEngine;

public class S_PlayerConfigurations : NetworkBehaviour
{
    [Networked] public PlayerRef playerOwner {  get; private set; }

    [SerializeField] GameObject realPlayer;
    [SerializeField] GameObject fakePlayer;

    void Start()
    {
        if (!Runner.IsPlayer) return;

        realPlayer.SetActive(false);
        fakePlayer.SetActive(false);

        if(playerOwner == Runner.LocalPlayer)
        {
            realPlayer.SetActive(true);
            Runner.SetPlayerObject(playerOwner, GetComponent<NetworkObject>());
            name = "My Player";
        }
        else
        {
            fakePlayer.SetActive(true);
            name = "Player";
        }

        if(Runner.IsServer)
        {
            //transform.position += transform.forward * 5;
        }
    }

    public void SetOwner(PlayerRef playerRef)
    {
        playerOwner = playerRef;
    }


}
