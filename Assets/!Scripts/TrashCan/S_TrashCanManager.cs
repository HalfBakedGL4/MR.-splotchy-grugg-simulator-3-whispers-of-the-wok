using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class S_TrashCanManager : NetworkBehaviour
{
    [SerializeField] private Transform suckPoint;
    [SerializeField] private GameObject platePrefab;

    [Header("Child Scripts")]
    [SerializeField] private S_DestroyTrash destroyTrash;
    [SerializeField] private S_MoveTrash moveTrash;
    [SerializeField] private S_PlateDispenser[] plateDispenserScripts;

    public override void Spawned()
    {
        base.Spawned();

        destroyTrash.enabled = true;
        moveTrash.enabled = true;

        // Add listener to event by the GameManager That calls CleanUpFloor
        if (!HasStateAuthority) return;
        S_GameManager.OnFoodListFull += CleanUpFloor;
        foreach (var plateDispenser in plateDispenserScripts)
        {
            plateDispenser.OnPlateRemoved += AddNewPlate;
            plateDispenser.FirstTimeSpawnPlate();
        }
    }


    // Whenever Game Manager Food list is full the trash will try to clean up the scene
    // Moves any food on the floor to the trash can
    private void CleanUpFloor()
    {
        var foodInScene = FindObjectsByType<S_Food>(FindObjectsSortMode.None);
        var dishInScene = FindObjectsByType<S_DishStatus>(FindObjectsSortMode.None);

        List<GameObject> foodOnFloor = new List<GameObject>();

        foreach (var food in foodInScene)
        {
            if (food.transform.position.y < .3f)
            {
                foodOnFloor.Add(food.gameObject);
            }
        }

        foreach (var dish in dishInScene)
        {
            if (dish.transform.position.y < .3f)
            {
                foodOnFloor.Add(dish.gameObject);
            }
        }

        // Make food on floor move towards suckPoint to be deleted
        StartCoroutine(SuckFoodCoroutine(foodOnFloor));
    }

    private IEnumerator SuckFoodCoroutine(List<GameObject> foodOnFloor)
    {
        List<Rigidbody> foodRBs = foodOnFloor.Select(food => food.GetComponent<Rigidbody>()).ToList();

        while (foodRBs.Count > 0)
        {
            foreach (var food in foodRBs.ToList().Where(food => food))
            {
                food.AddForce((suckPoint.position - food.transform.position).normalized * 5f);

                if (Vector3.Distance(food.transform.position, suckPoint.position) < 0.1f)
                {
                    foodRBs.Remove(food);
                }
            }

            yield return null; // wait one frame so physics can update
        }
    }


    private void AddNewPlate(Transform spawnPoint)
    {
        Runner.Spawn(platePrefab, spawnPoint.position, spawnPoint.rotation);
    }
    
    public virtual void OnDisable()
    {
        if (!HasStateAuthority) { return; }
        S_GameManager.OnFoodListFull -= CleanUpFloor;
        foreach (var plateDispenser in plateDispenserScripts)
        {
            plateDispenser.OnPlateRemoved -= AddNewPlate;
        }
    }
}
