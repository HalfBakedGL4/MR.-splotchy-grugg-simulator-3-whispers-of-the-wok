using UnityEngine;

public class Knife : MonoBehaviour
{
    private GameObject objectWithDamageScript;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Vegetable") 
        {
            objectWithDamageScript = col.gameObject;
            objectWithDamageScript.GetComponent<Health>().OnDamage?.Invoke();
            objectWithDamageScript.GetComponent<S_CuttingObjects>().OnChop?.Invoke();
        }
        if (col.tag == "Customer") 
        {
            objectWithDamageScript = col.gameObject;
            objectWithDamageScript.GetComponent<Health>().OnDamage?.Invoke();
        }
    }
}
