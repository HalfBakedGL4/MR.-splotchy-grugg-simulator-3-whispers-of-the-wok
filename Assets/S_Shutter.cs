using System.Collections;
using UnityEngine;

public class S_Shutter : MonoBehaviour
{
    [SerializeField, Min(1)] float shutterOpenCloseTime;
    bool isOpen;

    private void Update()
    {

        switch(S_GameManager.CurrentGameState )
        {
            case GameState.Starting:
                {
                    if (!isOpen)
                        MoveShutter(true);
                    break;
                }
            case GameState.Ongoing:
                {
                    if (!isOpen)
                        MoveShutter(true);
                    break;
                }
            case GameState.Ending:
                {
                    if (isOpen)
                        MoveShutter(false);
                    break;
                }
            case GameState.Intermission:
                {
                    if (isOpen)
                        MoveShutter(false);
                    break;
                }
        }
    }


    IEnumerator MoveShutter(bool open)
    {
        float opening = 0;

        if (open)
        {
            isOpen = true;
            Debug.Log("[Shutter] opening");
            while (opening < 1)
            {
                yield return new WaitForEndOfFrame();
                opening += Time.deltaTime / shutterOpenCloseTime;
                transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up, opening);
            }
        }
        else
        {
            isOpen = false;
            Debug.Log("[Shutter] closing");
            while (opening < 1)
            {
                yield return new WaitForEndOfFrame();
                opening += Time.deltaTime / shutterOpenCloseTime;
                transform.localPosition = Vector3.Lerp(Vector3.up, Vector3.zero, opening);
            }
        }


    }
}
