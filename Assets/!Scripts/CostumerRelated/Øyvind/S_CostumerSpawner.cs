using System.Collections.Generic;
using Fusion;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Random = UnityEngine.Random;

public class S_CostumerSpawner : NetworkBehaviour
{
    [SerializeField] List<GameObject> costumerPrefabs; // Prefabs to instantiate on collision
    [SerializeField] private float lengthInFrontOfHole = .2f;
    
    NetworkRunner runner;
    
    

    public void SpawnCostumer(Vector3 pos, ARPlane wall)
    {
        // Check if runner is there
        if (runner == null)
        {
            runner = FindAnyObjectByType<NetworkRunner>();
        }
        // Create costumer
        var costumerInstance = runner.Spawn(costumerPrefabs[Random.Range(0, costumerPrefabs.Count)], pos);
        
        // Make costumer child of wall to change its rotation compared to the wall and then release it from custody
        costumerInstance.transform.parent = wall.transform;
        costumerInstance.transform.localRotation = Quaternion.Euler(90, 0, 0);
        costumerInstance.transform.parent = null;

        // Place costumer in front of wall
        costumerInstance.transform.position += costumerInstance.transform.forward * lengthInFrontOfHole;
    }
}
