using UnityEngine;
using System.Collections;

public class Knife : MonoBehaviour
{
    private GameObject objectWithDamageScript;

    float countDown = 2f;
    bool isDamaged; 

    private void OnTriggerEnter(Collider col)
    {
        if (isDamaged) return; 

        if (col.tag == "Vegetable") 
        {
            StartCoroutine(Timer());

            objectWithDamageScript = col.transform.parent.gameObject;
            
            if (objectWithDamageScript.TryGetComponent<Health>(out Health health))
                health.ChopObject(col);
        }
        if (col.tag == "Customer") 
        {
            objectWithDamageScript = col.gameObject;
            objectWithDamageScript.GetComponent<Health>().Damage(1);
        }
    }

    
    IEnumerator Timer()
    {
        Debug.Log("LOLO");
        isDamaged = true;
        yield return new WaitForSeconds(countDown);
        isDamaged = false;
    }
}
