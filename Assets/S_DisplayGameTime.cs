using UnityEngine;
using TMPro;

public class S_DisplayGameTime : MonoBehaviour
{
    TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = S_GameManager.currentGameState.ToString();
        if (S_GameManager.currentGameState == GameState.Ongoing)
        {
            text.text = S_GameManager.GetGameTime();
        }
    }
}
