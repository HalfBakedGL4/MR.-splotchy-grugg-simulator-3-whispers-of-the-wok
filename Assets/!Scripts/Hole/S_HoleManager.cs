using System.Drawing;
using UnityEngine;
using Fusion;
using Oculus.Interaction.Samples;

public class HoleManager : NetworkBehaviour
{
    private void Start()
    {
        
    }

    //Hammer fix wall
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            RPCHammerHit();
        }
    }


    void RPCHammerHit()
    {
        // Shrinks hole by 20% each hit
        UpdateSize();

    }

    void UpdateSize()
    {
        if (transform.parent != null)
        {
            transform.parent.localScale = transform.parent.localScale * -0.4f;
        }
        else
        {
            transform.localScale = transform.localScale * -0.4f;
        }

        if (transform.localScale.x <= 0.2) { OnFixed(); }
    }

    void OnFixed()
    {
        Destroy(gameObject.transform.parent);
    }
}
