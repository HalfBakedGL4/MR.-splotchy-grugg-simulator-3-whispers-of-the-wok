using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon;
using Fusion;
using Input;
using TMPro;


public class S_HoleSpawner : NetworkBehaviour
{
    Camera cam => Camera.main; // Only used for raycast, remove later.
    [SerializeField] List<GameObject> holePrefabs; // Prefabs to instantiate on collision
    [SerializeField] private InputActionProperty inputAction;

    [Header("Fix multiple holes at once")]
    [SerializeField] bool multiHoleFix;

    private int holeIndex;
    private Transform parent;
    NetworkRunner runner;

    [SerializeField] private TMP_Text debug;
    
    private void Start()
    {
        runner = FindFirstObjectByType<NetworkRunner>();
    }
    /*
       public bool IsLocalNetworkRig => Object && Object.HasStateAuthority;

       private void Start()
       {
           if (!IsLocalNetworkRig) enabled = false;

           runner = FindFirstObjectByType<NetworkRunner>();
       }


       public void SpawnItem(InputInfo info)
       {
           if (!info.context.started) return;

           //CastRay(out RaycastHit hit);
           RPC_CreatePortal(hit.point, hit.transform.rotation);
       }

       void RPC_CreatePortal(Vector3 pos, Quaternion rot)
       {
           Debug.Log("Spawn Portal");
           SpawnHole(pos, rot);
       }

       // Raycast for testing, removed later when enemies are spawning the holes.
       private RaycastHit CastRay(out RaycastHit raycast)
       {
           Ray ray = new Ray(cam.transform.position, cam.transform.forward);

           if (Physics.Raycast(ray, out raycast))
           {

               Quaternion rot = Quaternion.Euler(0, raycast.transform.rotation.y, 0);
           }
           return raycast;
       }
       */

    public void SpawnHole(Vector3 pos, Quaternion rot)
    {
        debug.text += " trying to spawn hole ";

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
        debug.text += " after checking for naighboring holes ";

        // Select random hole prefab from list

        holeIndex = UnityEngine.Random.Range(0, holePrefabs.Count);
        debug.text += " Hole Index" + holeIndex;
        // rot.z = UnityEngine.Random.Range(0, 360);

        NetworkObject spawnedHole = runner.Spawn(holePrefabs[holeIndex], pos, rot);
        debug.text += " SpawnedHole " + spawnedHole.name;

        // Merges all hole under one parent, so all connected holes can be fixed at once.
        if (multiHoleFix && parent != null)
        {
            // Spawns hole and then parent it to keep its original size on spawn.
            spawnedHole.transform.parent = parent.transform;
        }

        parent = null;
    }


}
