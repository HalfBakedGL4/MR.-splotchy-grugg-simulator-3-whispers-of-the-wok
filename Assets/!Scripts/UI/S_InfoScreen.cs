using Fusion;
using TMPro;
using UnityEngine;

public class S_InfoScreen : MonoBehaviour
{
    [SerializeField] TMP_Text connectionText;
    [SerializeField] TMP_Text playerCount;

    private void Update()
    {
        connectionText.text = S_GameManager.currentGameState.ToString();

        if (S_GameManager.isConnected)
        {
            playerCount.text = S_GameManager.sessionInfo.PlayerCount + "/" + S_GameManager.sessionInfo.MaxPlayers;
        } else
        {
            playerCount.text = "";
        }
    }
}
