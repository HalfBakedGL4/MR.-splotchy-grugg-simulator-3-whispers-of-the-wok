using UnityEngine;

public class HoleManager : MonoBehaviour
{
    public GameObject parent;

    //Hammer fix wall
    private void OnCollisionEnter(Collision collision)
    {
        //On hammer hit
        if (collision.gameObject.CompareTag("Finish"));
        {
            HammerHit();
        }
    }

    void ParentCheck()
    {
        
    }

    void HammerHit()
    {
        Destroy(gameObject);

    }
}
