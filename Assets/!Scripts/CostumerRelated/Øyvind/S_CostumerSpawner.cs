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

    S_OrderWindow window;

    private void Start()
    {
        
    }
    public void SpawnCostumer(Vector3 pos, ARPlane wall)
    {
        window = FindFirstObjectByType<S_OrderWindow>();
        // Create costumer
        var costumerInstance = Runner.Spawn(costumerPrefabs[Random.Range(0, costumerPrefabs.Count)], window.transform.position);

        
        // Spawn Customer behind window
        
        Vector3 behindOffset = window.transform.forward * 1.5f;
        costumerInstance.transform.position = window.transform.position;

        
    }
}
