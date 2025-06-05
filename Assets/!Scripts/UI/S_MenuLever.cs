using Fusion;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class S_MenuLever : NetworkBehaviour
{
    [SerializeField] GameObject lever;

    [Networked] NetworkBool hasHappened { get; set; }

    const int amountToMove = 20;


    void OnTriggerEnter(Collider other)
    {
        if (hasHappened) return;

        Debug.Log("[Lever] Move");
        Vector3 toPlayer = (other.transform.position - transform.position).normalized;
        Vector3 right = transform.forward;

        float dot = Vector3.Dot(toPlayer, right);
        NetworkBool moveLeft;

        if (dot > 0)
        {
            moveLeft = false;
            Debug.Log("[Lever] Entered from the RIGHT");
        }
        else
        {
            moveLeft = true;
            Debug.Log("[Lever] Entered from the LEFT");
        }

        RPC_TryMoveLever(moveLeft);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_TryMoveLever(NetworkBool moveLeft)
    {
        StartCoroutine(MoveLever(moveLeft));
    }

    IEnumerator MoveLever(bool moveLeft)
    {
        Debug.Log("[Lever] moving");
        hasHappened = true;

        float rotatePosX = lever.transform.localEulerAngles.x +  360;

        rotatePosX += moveLeft ? amountToMove : -amountToMove;

        rotatePosX = Mathf.Clamp(rotatePosX - 360, -amountToMove, amountToMove) + 360;

        lever.transform.localEulerAngles = new Vector3(rotatePosX - 360, 0, 0);

        yield return StartCoroutine(S_SettingsMenu.instance.UpdateSelectedPlanet((Planet)((rotatePosX - 360) / 20)));

        hasHappened = false;
    }

}