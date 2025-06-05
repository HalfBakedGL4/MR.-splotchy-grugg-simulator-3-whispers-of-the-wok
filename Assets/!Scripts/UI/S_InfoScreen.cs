using Fusion;
using TMPro;
using UnityEngine;

public class S_InfoScreen : MonoBehaviour
{
    [SerializeField] TMP_Text connectionText;

    private void Update()
    {
        connectionText.text = S_GameManager.CurrentGameState.ToString();

        switch(S_GameManager.CurrentGameState)
        {
            case GameState.Offline:
                {
                    break;
                }
            case GameState.Intermission:
                {
                    if(!S_GameManager.ready)
                    {
                        connectionText.text = S_GameManager.sessionInfo.PlayerCount + "/" + S_GameManager.instance.playersRequired;
                    } else
                    {
                        connectionText.text = "Ready!";
                    }

                    break;
                }
            case GameState.Starting:
                {
                    break;
                }
            case GameState.Ongoing:
                {
                    connectionText.text = "Time Left: " + S_GameManager.currentGameTime;
                    break;
                }
            case GameState.Ending:
                {
                    break;
                }
        }
    }
}
