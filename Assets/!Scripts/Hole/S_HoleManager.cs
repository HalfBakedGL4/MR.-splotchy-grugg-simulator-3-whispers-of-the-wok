using System.Drawing;
using UnityEngine;
using Fusion;
using Oculus.Interaction.Samples;

public class S_HoleManager : NetworkBehaviour
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
            //if(!runner.IsSharedModeMasterClient) return;
            RPCHammerHit(0.4f);
        }
    }



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPCHammerHit(float charge)
    {
        if (!Object.HasStateAuthority) return;
        size -= charge;
        Debug.Log("Recieved charge " + charge);
        Debug.Log("Hole Size: "+size);
        UpdateSize();

    }

    void UpdateSize()
    {
        if (transform.parent != null)
        {
            transform.parent.localScale = transform.parent.localScale * size;
        }
        else
        {
            transform.localScale = transform.localScale * size;
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
