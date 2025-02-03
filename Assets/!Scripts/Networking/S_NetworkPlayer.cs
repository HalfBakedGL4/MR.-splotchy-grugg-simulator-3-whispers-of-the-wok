using Fusion;
using System;
using System.Threading.Tasks;
using UnityEngine;

[DefaultExecutionOrder(S_NetworkPlayer.EXECUTION_ORDER)]
public class S_NetworkPlayer : NetworkBehaviour
{
    public const int EXECUTION_ORDER = 100;
    [SerializeField] S_LocalPlayer connectedPlayer;

    [SerializeField] S_NetworkPart head, rightHand, leftHand;

    public bool IsLocalNetworkRig => Object.HasInputAuthority;

    public override void Spawned()
    {
        base.Spawned();

        if (!IsLocalNetworkRig) return;

        name = "my NetworkPlayer";

        connectedPlayer = FindAnyObjectByType<S_LocalPlayer>();
        Debug.Log(connectedPlayer);

        //Head.GetComponent<MeshRenderer>().enabled = false;
        //RightHand.GetComponent<MeshRenderer>().enabled = false;
        //LeftHand.GetComponent<MeshRenderer>().enabled = false;

        if (connectedPlayer == null)
            Debug.LogError("No LocalPlayer in scene");
    }

    public override void Render()
    {
        if (!IsLocalNetworkRig || connectedPlayer == null) return;

        transform.position = connectedPlayer.transform.position;
        transform.rotation = connectedPlayer.transform.rotation;

        head.transform.position = connectedPlayer.head.transform.position;
        head.transform.rotation = connectedPlayer.head.transform.rotation;

        rightHand.transform.position = connectedPlayer.rightHand.transform.position;
        rightHand.transform.rotation = connectedPlayer.rightHand.transform.rotation;

        leftHand.transform.position = connectedPlayer.leftHand.transform.position;
        leftHand.transform.rotation = connectedPlayer.leftHand.transform.rotation;
    }
}
