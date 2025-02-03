using Fusion;
using System;
using System.Threading.Tasks;
using UnityEngine;

[DefaultExecutionOrder(S_NetworkPlayer.EXECUTION_ORDER)]
public class S_NetworkPlayer : NetworkBehaviour
{
    public const int EXECUTION_ORDER = 100;
    [SerializeField] S_LocalPlayer ConnectedPlayer;

    [SerializeField] S_NetworkPart Head, RightHand, LeftHand;

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
        if (!IsLocalNetworkRig || ConnectedPlayer == null) return;

        transform.position = ConnectedPlayer.transform.position;
        transform.rotation = ConnectedPlayer.transform.rotation;

        Head.transform.position = ConnectedPlayer.Head.transform.position;
        Head.transform.rotation = ConnectedPlayer.Head.transform.rotation;

        RightHand.transform.position = ConnectedPlayer.RightHand.transform.position;
        RightHand.transform.rotation = ConnectedPlayer.RightHand.transform.rotation;

        LeftHand.transform.position = ConnectedPlayer.LeftHand.transform.position;
        LeftHand.transform.rotation = ConnectedPlayer.LeftHand.transform.rotation;
    }
}
