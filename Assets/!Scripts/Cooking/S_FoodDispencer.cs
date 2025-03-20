using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_FoodDispencer : MonoBehaviour, IButtonObject
{
    [SerializeField] private S_Food foodToDispense;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform hoverPoint;

    [SerializeField] private float launchSpeed;
    [SerializeField] private float rotationSpeed;

    /*
    private void Start()
    {
        // Spawn Object to be Dispensed over machine ig
        var hoverItem = Instantiate(foodToDispense, hoverPoint.position, Quaternion.identity);
        hoverItem.GetComponent<Rigidbody>().isKinematic = true;
        hoverItem.GetComponent<Collider>().enabled = false;
    }*/


    public void OnButtonPressed()
    {
        var foodItem = Instantiate(foodToDispense, spawnPoint.position, Quaternion.identity);

        if (foodItem.TryGetComponent(out Rigidbody rb))
        {
            // Launches the item forward
            rb.linearVelocity = spawnPoint.transform.forward * launchSpeed;
            
            // Set random angular velocity
            rb.angularVelocity = Random.insideUnitSphere * rotationSpeed;
        }
    }
}
