using System;
using UnityEngine;

public enum FoodType
{
    Onion,
    Fish,
    Bread,
    Potato,
}
public class Food : MonoBehaviour
{
    
    [SerializeField] FoodType foodType;
    
    private Collider coll;
    private Rigidbody rb;
    
    private void Start()
    {
        coll = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public FoodType GetFoodType()
    {
        return foodType;
    }
    public void TurnOffPhysics()
    {
        coll.enabled = false;
        rb.isKinematic = true;
    }

    public void TurnOnPhysics()
    {
        coll.enabled = true;
        rb.isKinematic = false;
    }
    
}
