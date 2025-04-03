using System.Drawing;
using UnityEngine;
using Fusion;
using Oculus.Interaction.Samples;

public class S_HoleManager : MonoBehaviour
{
    [Networked, OnChangedRender(nameof(UpdateSize))]
    public float size { get; set; }

    private void Start()
    {
        size = transform.localScale.x;
    }

    //Hammer fix wall
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            RPCHammerHit();
        }
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPCHammerHit()
    {
        // Shrinks hole by 20% each hit
        size -= 0.4f;

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

        if (size <= 0.2) { OnFixed(); }
    }

    void OnFixed()
    {
        Destroy(gameObject.transform.parent);
    }
}