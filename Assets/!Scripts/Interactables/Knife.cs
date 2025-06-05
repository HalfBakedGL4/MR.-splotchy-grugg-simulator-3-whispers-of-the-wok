using UnityEngine;
using System.Collections;
using Fusion;

public class Knife : NetworkBehaviour
{
    private GameObject objectWithDamageScript;

    private bool isLocal => Object && Object.HasStateAuthority;

    float countDown = 0.2f;
    [Networked] private bool isDamaged { get; set; }
    

    private void OnTriggerEnter(Collider col)
    {
        if (!isLocal) return;

        if (col.CompareTag("Vegetable")) 
        {
            if (isDamaged) return; 


            objectWithDamageScript = col.transform.parent.gameObject;
            
            if (objectWithDamageScript.TryGetComponent(out Health health))
                health.ChopObject(col);
        }
        
        if (col.CompareTag("Customer")) 
        {
            if (isDamaged) return; 

            objectWithDamageScript = col.gameObject;
            objectWithDamageScript.GetComponent<Health>().Damage(1);
            Debug.LogWarning("\n" + name + " hit Customer: " + col.gameObject.name);
        }
        
        StartCoroutine(Timer());

    }

    
    IEnumerator Timer()
    {
        isDamaged = true;
        yield return new WaitForSeconds(countDown);
        isDamaged = false;
    }
}
