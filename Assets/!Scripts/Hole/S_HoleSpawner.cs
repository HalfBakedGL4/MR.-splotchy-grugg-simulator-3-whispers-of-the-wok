using System.Collections.Generic;
using UnityEngine;
using Photon;
using Fusion;



public class S_HoleSpawner : NetworkBehaviour
{
    
    [SerializeField] List<GameObject> holePrefabs; // Prefabs to instantiate on collision

    [Header("Fix multiple holes at once")]
    [SerializeField] bool multiHoleFix;

    private int holeIndex;
    private Transform parent;
    NetworkRunner runner;

    
    /*
        Camera cam => Camera.main; // Only used for raycast, remove later.
        [SerializeField] private InputActionProperty inputAction;
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
        holeIndex = UnityEngine.Random.Range(0, holePrefabs.Count);
        
        //rot.z = UnityEngine.Random.Range(0, 360);
        
        // Check if runner is found
        if (runner == null)
        {
            runner = FindAnyObjectByType<NetworkRunner>();
        }
        // Spawns hole
        NetworkObject spawnedHole = runner.Spawn(holePrefabs[holeIndex], pos, rot);

        // Merges all hole under one parent, so all connected holes can be fixed at once.
        if (multiHoleFix && parent != null)
        {
            // Spawns hole and then parent it to keep its original size on spawn.
            spawnedHole.transform.parent = parent.transform;
        }

        parent = null;
    }


}
