using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class S_TrashCanManager : NetworkBehaviour
{
    [SerializeField] private Transform suckPoint;
    
    [Header("Child Scripts")]
    [SerializeField] private S_DestroyTrash destroyTrash;
    [SerializeField] private S_MoveTrash moveTrash;
    
    public override void Spawned()
    {
        base.Spawned();
        
        destroyTrash.enabled = true;
        moveTrash.enabled = true;
        
        // Add listener to event by the GameManager That calls CleanUpFloor
        S_GameManager.OnFoodListFull += CleanUpFloor;
    }

    // Whenever Game Manager Food list is full the trash will try to clean up the scene
    // Moves any food on the floor to the trash can
    private void CleanUpFloor()
    {
        var foodInScene = FindObjectsByType<S_Food>(FindObjectsSortMode.None);

        List<S_Food> foodOnFloor = new List<S_Food>();
        
        foreach (var food in foodInScene)
        {
            if (food.transform.position.y < .3f)
            {
                foodOnFloor.Add(food);
            }
        }

        // Make food on floor move towards suckPoint to be deleted
        StartCoroutine(SuckFoodCoroutine(foodOnFloor));
    }
    
    private IEnumerator SuckFoodCoroutine(List<S_Food> foodOnFloor)
    {
        List<Rigidbody> foodRBs = new List<Rigidbody>();

        foreach (var food in foodOnFloor)
        {
            foodRBs.Add(food.GetComponent<Rigidbody>());
        }

        while (foodRBs.Count > 0)
        {
            foreach (var food in foodRBs.ToList())
            {
                if (food == null) continue;

                food.AddForce((suckPoint.position - food.transform.position).normalized * 5f); 

                if (Vector3.Distance(food.transform.position, suckPoint.position) < 0.1f)
                {
                    foodRBs.Remove(food);
                }
            }

            yield return null; // wait one frame so physics can update
        }
    }

    public virtual void OnDisable()
    {
        S_GameManager.OnFoodListFull -= CleanUpFloor;
    }
}
