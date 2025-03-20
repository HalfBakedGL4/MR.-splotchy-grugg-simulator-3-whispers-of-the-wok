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

            objectWithDamageScript = col.gameObject;
            objectWithDamageScript.GetComponent<Health>().Damage(1);

            //objectWithDamageScript.GetComponent<S_CuttingObjects>().TryChop();
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
