using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class S_StartGame : MonoBehaviour, IButtonObject
{
    [SerializeField] Transform Shutter;
    [SerializeField, Min(1)] float shutterOpenCloseTime;

    Vector3 shutterEndPos = Vector3.up;

    [Button]
    public void OnButtonPressed()
    {
        if(S_GameManager.StartGame())
        {
            StartCoroutine(MoveShutter(true));
            S_GameManager.instance.OnEnding.AddListener(CloseShutter);
        }
    }
    void CloseShutter(S_GameManager gameManager)
    {
        StartCoroutine(MoveShutter(false));
        S_GameManager.instance.OnEnding.RemoveListener(CloseShutter);
    }

    IEnumerator MoveShutter(bool open)
    {
        float opening = 0;

        if (open)
        {
            Debug.Log("[GameStartButton] opening");
            while (opening < 1)
            {
                yield return new WaitForEndOfFrame();
                opening += Time.deltaTime / shutterOpenCloseTime;
                Shutter.localPosition = Vector3.Lerp(Vector3.zero, shutterEndPos, opening);
            }
        } 
        else
        {
            Debug.Log("[GameStartButton] closing");
            while (opening < 1)
            {
                yield return new WaitForEndOfFrame();
                opening += Time.deltaTime / shutterOpenCloseTime;
                Shutter.localPosition = Vector3.Lerp(shutterEndPos, Vector3.zero, opening);
            }
        }


    }

}
