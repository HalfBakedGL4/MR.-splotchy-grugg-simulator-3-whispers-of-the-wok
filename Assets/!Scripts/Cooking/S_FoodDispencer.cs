using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;

public class S_FoodDispencer : MonoBehaviour, IButtonObject
{
    [SerializeField] private S_Food foodToDispense;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform hoverPoint;

    [SerializeField] private float launchSpeed;
    [SerializeField] private float rotationSpeed;

    private GameObject hoverItem;
    
    private void Start()
    {
        // Spawn Object to be Displayed over machine, to show what it spawns ig
        var itemInstance = Instantiate(foodToDispense, hoverPoint.position, Quaternion.identity);
        hoverItem = itemInstance.gameObject;
        hoverItem.GetComponent<Rigidbody>().isKinematic = true;
        hoverItem.GetComponent<Collider>().enabled = false;
        hoverItem.GetComponent<XRGrabInteractable>().enabled = false;
        hoverItem.GetComponent<S_Food>().enabled = false;
        hoverItem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    private void Update()
    {
        // Spins the hover Item
        hoverItem.transform.Rotate(30 * Time.deltaTime,15 * Time.deltaTime,10 * Time.deltaTime);
    }
    
    public void OnButtonPressed() // When button is pressed spawn and launch the selected food Item
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
