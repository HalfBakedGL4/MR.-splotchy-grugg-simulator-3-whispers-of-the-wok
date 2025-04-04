using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public enum FoodType
{
    // All types of food used to create dishes,
    // add more at the BOTTOM when needed
    Onion,
    Fish,
    Bread,
    Potato,
}
public class S_Food : MonoBehaviour
{
    
    [SerializeField] private FoodType foodType;
    private XRGrabInteractable grabInteractable;
    
    private Collider coll;
    
    private void Start()
    {
        coll = GetComponent<Collider>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public FoodType GetFoodType()
    {
        return foodType;
    }
    
    // Turn on and off grab interactable script to move away from socket
    public void TurnOffGrab()
    {
        grabInteractable.enabled = false;
    }
    public void TurnOnGrab()
    {
        grabInteractable.enabled = true;
    }
    
    // Turn on and off collider so that player can't grab object
    public void TurnOffColliders()
    {
        coll.enabled = false;
    }
    public void TurnOnColliders()
    {
        coll.enabled = true;
    }
    
}
