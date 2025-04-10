using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public enum FoodType
{
    // All types of food used to create dishes,
    // add more at the BOTTOM when needed
    None = 0,
    Onion = 1,
    Fish = 2,
    Bread = 3
}
public class S_Food : MonoBehaviour
{
    
    [SerializeField] private FoodType foodType;
    private XRGrabInteractable grabInteractable;
    
    private Collider coll;
    MeshRenderer[] renderers;
    
    private void Start()
    {
        coll = GetComponent<Collider>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    public FoodType GetFoodType()
    {
        return foodType;
    }

    /// <summary>
    /// toggle grab, colliders and visuals
    /// </summary>
    /// <param name="toggle">enable or disable</param>
    public void Toggle(bool toggle = false)
    {
        ToggleGrab(toggle);
        ToggleColliders(toggle);
        ToggleVisuals(toggle);
    }

    /// <summary>
    /// Turn on and off grab interactable script to move away from socket
    /// </summary>
    /// <param name="toggle">enable or disable</param>
    public void ToggleGrab(bool toggle = false)
    {
        grabInteractable.enabled = toggle;
    }

    /// <summary>
    /// Turn on and off collider so that player can't grab object
    /// </summary>
    /// <param name="toggle">enable or disable</param>
    public void ToggleColliders(bool toggle = false)
    {
        coll.enabled = toggle;
    }
    /// <summary>
    /// Turn on and off the visuals
    /// </summary>
    /// <param name="toggle">enable or disable</param>
    public void ToggleVisuals(bool toggle = false)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = toggle;
        }
    }
    
}
