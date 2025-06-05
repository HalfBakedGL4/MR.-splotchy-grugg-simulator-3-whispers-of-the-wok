using Fusion;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class S_MenuLever : MonoBehaviour
{
    [SerializeField] GameObject lever;
    bool moveLeft;

    bool hasHappened;

    const int amountToMove = 20;


    void OnTriggerEnter(Collider other)
    {
        if (hasHappened) return;

        Debug.Log("[Lever] Move");
        Vector3 toPlayer = (other.transform.position - transform.position).normalized;
        Vector3 right = transform.forward;

        float dot = Vector3.Dot(toPlayer, right);

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


        TryMoveLever();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void TryMoveLever()
    {
        MoveLever();
    }

    IEnumerator MoveLever()
    {
        hasHappened = true;

        float rotatePosX = lever.transform.localEulerAngles.x +  360;

        rotatePosX += moveLeft ? amountToMove : -amountToMove;

        rotatePosX = Mathf.Clamp(rotatePosX - 360, -amountToMove, amountToMove) + 360;

        lever.transform.localEulerAngles = new Vector3(rotatePosX - 360, 0, 0);

        yield return StartCoroutine(S_SettingsMenu.instance.UpdateSelectedPlanet( (Planet)((rotatePosX - 360) / 20) ));

        hasHappened = false;
    }

}