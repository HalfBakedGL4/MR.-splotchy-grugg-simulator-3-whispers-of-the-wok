using UnityEngine;

public class S_StartGame : MonoBehaviour, IButtonObject
{
    public void OnButtonPressed()
    {
        S_GameManager.StartGame();
    }

}
