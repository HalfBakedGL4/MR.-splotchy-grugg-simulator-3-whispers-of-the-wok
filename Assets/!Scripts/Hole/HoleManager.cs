using System.Drawing;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    private float size = 1;

    private void Start()
    {
        size = transform.localScale.x;
    }

    //Hammer fix wall
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            HammerHit();
        }
    }


    void HammerHit()
    {
        // Shrinks hole by 20% each hit
        size -= 0.4f;
        if (transform.parent != null)
        {
            transform.parent.localScale = transform.parent.localScale * size;
        }
        else 
        { 
            transform.localScale = transform.localScale * size; 
        }

        if(size <= 0.2) { OnFixed(); }
    }

    void OnFixed()
    {
        Destroy(gameObject.transform.parent);
    }
}
