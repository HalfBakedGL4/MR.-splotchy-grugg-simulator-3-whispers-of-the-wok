using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoleSpawner : MonoBehaviour
{
    [SerializeField] Camera cam; // Only used for raycast, remove later.
    [SerializeField] List<GameObject> holePrefabs; // Prefabs to instantiate on collision
    [SerializeField] private InputActionProperty inputAction;
    
    [Header("Fix multiple holes at once")]
    [SerializeField] bool multiHoleFix;

    private Transform parent;


    void Update()
    {
        // Ray for debugging and testing
        if (inputAction.action.WasPressedThisFrame())
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                
                Quaternion rot = Quaternion.Euler(0,hit.transform.rotation.y,0);
                SpawnHole(hit.point, hit.transform.rotation);
                
            }
        }
    }
    
    void SpawnHole(Vector3 pos, Quaternion rot)
    {
        // Checks for holes nearby.
        Collider[] hitColliders = Physics.OverlapSphere(pos, 1, LayerMask.GetMask("Default"), QueryTriggerInteraction.Collide);
        foreach (var hitCollider in hitColliders)
        {
            // Parents last found hole
            if (hitCollider.CompareTag("Hole"))
            {
                parent = hitCollider.transform;
            }
        }

        // Select random hole prefab from list
        int holeIndex = UnityEngine.Random.Range(0, holePrefabs.Count);

        // rot.z = UnityEngine.Random.Range(0, 360);

        // Merges all hole under one parent, so all connected holes can be fixed at once.
        if (multiHoleFix && parent != null)
        {
            // Spawns hole and then parent it to keep its original size on spawn.
            GameObject spawnedHole = Instantiate(holePrefabs[holeIndex], pos, rot);
            spawnedHole.transform.parent = parent.transform;
        }
        // Seperate holes not connected.
        else
        {
            GameObject spawnedHole = Instantiate(holePrefabs[holeIndex], pos, rot);
        }
        
        parent = null;
    }


}
