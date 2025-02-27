using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private GameObject knife;

    private void OnTriggerEnter(Collider other) {
        if (tag == "Knife") 
        {
            knife.SetActive(true);
            other.transform.parent.gameObject.SetActive(false);
        }
    }
}
