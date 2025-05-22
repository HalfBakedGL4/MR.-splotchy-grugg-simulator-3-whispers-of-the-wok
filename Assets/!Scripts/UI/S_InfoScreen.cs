using Fusion;
using TMPro;
using UnityEngine;

public class S_InfoScreen : MonoBehaviour
{
    [SerializeField] TMP_Text connectionText;

    private void Update()
    {
        Debug.Log(S_GameManager.CurrentGameState);

        connectionText.text = S_GameManager.CurrentGameState.ToString();

        switch(S_GameManager.CurrentGameState)
        {
            case GameState.Offline:
                {
                    break;
                }
            case GameState.Intermission:
                {
                    connectionText.text += "\n" + S_GameManager.sessionInfo.PlayerCount + "/" + S_GameManager.instance.playersRequired;
                    break;
                }
            case GameState.Starting:
                {
                    connectionText.text += "\n" + S_GameManager.instance.delay.ToString("0");
                    break;
                }
            case GameState.Ongoing:
                {
                    connectionText.text += "\n" + S_GameManager.GetGameTime();
                    break;
                }
            case GameState.Ending:
                {
                    break;
                }
        }
    }
}
