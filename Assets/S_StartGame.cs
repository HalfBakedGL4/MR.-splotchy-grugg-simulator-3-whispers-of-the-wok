using NaughtyAttributes;
using UnityEngine;

public class S_StartGame : MonoBehaviour, IButtonObject
{
    [Button]
    public void OnButtonPressed()
    {
        S_GameManager.StartGame();
    }

}
