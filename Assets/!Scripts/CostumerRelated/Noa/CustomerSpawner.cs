using System.Collections.Generic;
using Fusion;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Random = UnityEngine.Random;

public class CustomerSpawner : NetworkBehaviour
{
    [SerializeField] List<GameObject> costumerPrefabs; // Prefabs to instantiate on collision
    [SerializeField] private float lengthInFrontOfHole = .2f;

    public void SpawnCostumer(Vector3 pos, ARPlane wall)
    {
        // Create costumer
        var costumerInstance = Runner.Spawn(costumerPrefabs[Random.Range(0, costumerPrefabs.Count)], pos);

        // Spawn Customer behind window
        S_OrderWindow window = FindFirstObjectByType<S_OrderWindow>();
        costumerInstance.transform.position = -window.transform.forward;

        // Place costumer in front of wall
        costumerInstance.transform.position += costumerInstance.transform.forward * lengthInFrontOfHole;
    }
}
