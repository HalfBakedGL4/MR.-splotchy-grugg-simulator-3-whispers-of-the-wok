using Fusion;
using System;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(NetworkTransform))]
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

        connectedPlayer = transform.parent.gameObject.GetComponentInChildren<S_LocalPlayer>();

        name = Runner.LocalPlayer.ToString() + "'s network player";

        head.GetComponent<MeshRenderer>().enabled = false;
        rightHand.GetComponent<MeshRenderer>().enabled = false;
        leftHand.GetComponent<MeshRenderer>().enabled = false;

        if (connectedPlayer == null)
            Debug.LogError("No LocalPlayer in scene");
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (GetInput<RigInput>(out var input))
        {
            print(input.rightHandPosition);

            transform.position = input.playAreaPosition;
            transform.rotation = input.playAreaRotation;
            leftHand.transform.position = input.leftHandPosition;
            leftHand.transform.rotation = input.leftHandRotation;
            rightHand.transform.position = input.rightHandPosition;
            rightHand.transform.rotation = input.rightHandRotation;
            head.transform.position = input.headPosition;
            head.transform.rotation = input.headRotation;
        }
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
