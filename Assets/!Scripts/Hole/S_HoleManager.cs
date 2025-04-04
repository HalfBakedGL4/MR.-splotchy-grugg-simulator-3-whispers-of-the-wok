using System.Drawing;
using UnityEngine;
using Fusion;
using Oculus.Interaction.Samples;

public class HoleManager : NetworkBehaviour
{
    [Networked] public float size {get; set;}


    NetworkRunner runner;

    public bool IsLocalNetworkRig => Object && Object.HasStateAuthority;

    private void Start()
    {
        size = 1;

        if (!IsLocalNetworkRig) enabled = false;

        runner = FindFirstObjectByType<NetworkRunner>();
    }

    //Hammer fix wall
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            if(!runner.IsSharedModeMasterClient) return;
            RPCHammerHit();
        }
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPCHammerHit()
    {
        if (!Object.HasStateAuthority) return;
        // Shrinks hole by 20% each hit
        size -= 0.3f;
        Debug.Log("Hole Size: "+size);
        UpdateSize();

    }

    void UpdateSize()
    {
        if (transform.parent != null)
        {
            transform.parent.localScale = transform.parent.localScale * 0.6f;
        }
        else
        {
            transform.localScale = transform.localScale * 0.6f;
        }

        if (size <= 0.2) 
        { 
            RPCOnFixed(); 
            Debug.Log("Size less than 0.2");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPCOnFixed()
    {
        if (!Object.HasStateAuthority) return;
        if (transform.parent != null)
        {
            runner.Despawn(gameObject.transform.parent.GetComponent<NetworkObject>());
        }
        else
        {
            runner.Despawn(gameObject.transform.GetComponent<NetworkObject>());
        }
        
    }

}
