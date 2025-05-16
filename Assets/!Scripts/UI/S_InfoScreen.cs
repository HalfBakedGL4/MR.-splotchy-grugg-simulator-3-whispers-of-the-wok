using Fusion;
using TMPro;
using UnityEngine;

public class S_InfoScreen : MonoBehaviour
{
    [SerializeField] TMP_Text connectionText;

    private void Update()
    {
        connectionText.text = S_GameManager.CurrentGameState.ToString();

        if (S_GameManager.CurrentGameState != GameState.Offline)
        {
            connectionText.text += "\n" + S_GameManager.sessionInfo.PlayerCount + "/" + S_GameManager.sessionInfo.MaxPlayers;
        }
    }
}
