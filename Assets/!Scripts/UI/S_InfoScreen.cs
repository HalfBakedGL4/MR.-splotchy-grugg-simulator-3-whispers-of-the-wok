using Fusion;
using TMPro;
using UnityEngine;

public class S_InfoScreen : MonoBehaviour
{
    NetworkRunner runner;
    [SerializeField] TMP_Text connectionText;
    [SerializeField] TMP_Text playerCount;

    private void Start()
    {
        runner = FindFirstObjectByType<NetworkRunner>();
    }

    private void Update()
    {
        if (runner.SessionInfo.PlayerCount > 0)
        {
            connectionText.text = "You are connected!";
            playerCount.text = runner.SessionInfo.PlayerCount + "/" + runner.SessionInfo.MaxPlayers;
        } else
        {
            connectionText.text = "You are not connected.";
            playerCount.text = "0/5";
        }
    }
}
