using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;

public class S_FoodDispenser : MonoBehaviour, IButtonObject
{
    [Header("Food Dispenser")]
    [SerializeField] private S_Food foodToDispense;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float launchSpeed;
    [SerializeField] private float rotationSpeed;
    
    [Header("Hover Item showing food Dispensed")]
    [SerializeField] private Transform hoverPoint;
    [SerializeField] private float hoverItemSize = 0.1f;
    

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
        hoverItem.transform.localScale = new Vector3(hoverItemSize, hoverItemSize, hoverItemSize);
    }

    private void Update()
    {
        var t = Time.deltaTime;
        // Spins the hover Item
        hoverItem.transform.Rotate(30 * t, 15 * t, 10 * t);
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
