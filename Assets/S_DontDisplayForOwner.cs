using Fusion;
using UnityEngine;

public class S_DontDisplayForOwner : NetworkBehaviour
{
    MeshRenderer mesh;

    public bool IsLocalNetworkRig => Object.HasInputAuthority;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();

        if (IsLocalNetworkRig)
        {
            Debug.Log("wont display");
            mesh.enabled = false;
        }

    }

    public override void Spawned()
    {
        base.Spawned();

        mesh = GetComponent<MeshRenderer>();

        if(IsLocalNetworkRig)
        {
            Debug.Log("wont display");
            mesh.enabled = false;
        }
    }
}
