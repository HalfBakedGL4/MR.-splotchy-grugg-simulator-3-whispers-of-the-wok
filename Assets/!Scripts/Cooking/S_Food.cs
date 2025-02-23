using System;
using UnityEngine;

public enum FoodType
{
    // All types of food used to create dishes, add more at the BOTTOM if needed
    Onion,
    Fish,
    Bread,
    Potato,
}
public class S_Food : MonoBehaviour
{
    
    [SerializeField] FoodType foodType;
    
    private Collider coll;
    
    private void Start()
    {
        coll = GetComponent<Collider>();
    }

    public FoodType GetFoodType()
    {
        return foodType;
    }
    public void TurnOffPhysics()
    {
        coll.enabled = false;
    }

    public void TurnOnPhysics()
    {
        coll.enabled = true;
    }
    
}
