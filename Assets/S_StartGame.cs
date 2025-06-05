using Fusion;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class S_StartGame : NetworkBehaviour, IButtonObject
{
    [SerializeField] Transform Shutter;

    public void OnButtonPressed()
    {
        S_GameManager.StartGame();
    }

}
