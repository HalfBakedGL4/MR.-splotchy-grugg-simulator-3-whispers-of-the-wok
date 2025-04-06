using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_CostumerSpawner : NetworkBehaviour
{
    [SerializeField] List<GameObject> costumerPrefabs; // Prefabs to instantiate on collision
    [SerializeField] private float lengthInFrontOfHole = .2f;
    
    NetworkRunner runner;
    
    

    public void SpawnCostumer(Vector3 pos, Quaternion rot)
    {
        if (runner == null)
        {
            runner = FindAnyObjectByType<NetworkRunner>();
        }
        
        var costumerInstance = runner.Spawn(costumerPrefabs[Random.Range(0, costumerPrefabs.Count)], pos, rot);

        costumerInstance.transform.position += costumerInstance.transform.forward * lengthInFrontOfHole;
    }
}
