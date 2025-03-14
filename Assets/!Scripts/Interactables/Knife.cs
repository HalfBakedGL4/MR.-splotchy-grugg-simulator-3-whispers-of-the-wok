using UnityEngine;

public class Knife : MonoBehaviour
{
    private GameObject objectWithDamageScript;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Tomato") 
        {
            objectWithDamageScript = col.gameObject;
            objectWithDamageScript.GetComponent<Health>().OnDamage?.Invoke();
        }
        if (col.tag == "Customer") 
        {
            objectWithDamageScript = col.gameObject;
            objectWithDamageScript.GetComponent<Health>().OnDamage?.Invoke();
        }
    }
}
