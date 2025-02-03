using Fusion;
using System;
using System.Threading.Tasks;
using UnityEngine;

[DefaultExecutionOrder(S_NetworkPlayer.EXECUTION_ORDER)]
public class S_NetworkPlayer : NetworkBehaviour
{
    public const int EXECUTION_ORDER = 100;
    [SerializeField] S_LocalPlayer ConnectedPlayer;

    [SerializeField] Transform Head, RightHand, LeftHand;

    public bool IsLocalNetworkRig => Object.HasInputAuthority;

    public override void Spawned()
    {
        base.Spawned();

        if (!IsLocalNetworkRig) return;

        name = "my NetworkPlayer";

        ConnectedPlayer = FindAnyObjectByType<S_LocalPlayer>();
        Debug.Log(ConnectedPlayer);

        //Head.GetComponent<MeshRenderer>().enabled = false;
        //RightHand.GetComponent<MeshRenderer>().enabled = false;
        //LeftHand.GetComponent<MeshRenderer>().enabled = false;

        if (ConnectedPlayer == null)
            Debug.LogError("No LocalPlayer in scene");
    }

    public override void Render()
    {
        if (!IsLocalNetworkRig) return;

        transform.position = ConnectedPlayer.rig.Body.position;
        transform.rotation = ConnectedPlayer.rig.Body.rotation;

        Head.transform.localPosition = ConnectedPlayer.rig.Head.localPosition;
        Head.transform.localRotation = ConnectedPlayer.rig.Head.localRotation;

        RightHand.transform.localPosition = ConnectedPlayer.rig.RightHand.localPosition;
        RightHand.transform.localRotation = ConnectedPlayer.rig.RightHand.localRotation;

        LeftHand.transform.localPosition = ConnectedPlayer.rig.LeftHand.localPosition;
        LeftHand.transform.localRotation = ConnectedPlayer.rig.LeftHand.localRotation;
    }
}
