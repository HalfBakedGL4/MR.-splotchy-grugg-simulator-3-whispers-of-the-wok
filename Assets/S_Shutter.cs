using Fusion;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class S_Shutter : NetworkBehaviour, IButtonObject
{
    [SerializeField] Transform Shutter;
    [SerializeField, Min(1)] float shutterOpenCloseTime;

    public void OnButtonPressed()
    {
        if(S_GameManager.StartGame())
        {
            RPC_MoveShutter(true);
            S_GameManager.instance.OnEnding.AddListener(CloseShutter);
        }
    }
    void CloseShutter(S_GameManager gameManager)
    {
        RPC_MoveShutter(false);
        S_GameManager.instance.OnEnding.RemoveListener(CloseShutter);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_MoveShutter(NetworkBool open)
    {
        StartCoroutine(MoveShutter(open));
    }

    IEnumerator MoveShutter(bool open)
    {
        float opening = 0;

        if (open)
        {
            Debug.Log("[Shutter] opening");
            while (opening < 1)
            {
                yield return new WaitForEndOfFrame();
                opening += Time.deltaTime / shutterOpenCloseTime;
                Shutter.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up, opening);
            }
        } 
        else
        {
            Debug.Log("[Shutter] closing");
            while (opening < 1)
            {
                yield return new WaitForEndOfFrame();
                opening += Time.deltaTime / shutterOpenCloseTime;
                Shutter.localPosition = Vector3.Lerp(Vector3.up, Vector3.zero, opening);
            }
        }


    }

}
