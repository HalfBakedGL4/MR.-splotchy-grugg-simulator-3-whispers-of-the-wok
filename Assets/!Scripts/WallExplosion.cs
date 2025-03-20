using UnityEngine;

public class WallExplosion : MonoBehaviour
{
    private Rigidbody[] rb;
    
    private void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();

        foreach (var rb in rb)
        {
            rb.AddExplosionForce(200, transform.position, 2);
        }
    }


}
