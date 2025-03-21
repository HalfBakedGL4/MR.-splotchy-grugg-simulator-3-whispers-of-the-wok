using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon;
using Fusion;

public class HoleSpawner : NetworkBehaviour
{
    [SerializeField] Camera cam; // Only used for raycast, remove later.
    [SerializeField] List<GameObject> holePrefabs; // Prefabs to instantiate on collision
    [SerializeField] private InputActionProperty inputAction;

    [Header("Fix multiple holes at once")]
    [SerializeField] bool multiHoleFix;

    private Transform parent;
    NetworkRunner runner;

    public bool IsLocalNetworkRig => Object.HasInputAuthority;

    private void Start()
    {
        if (!IsLocalNetworkRig) enabled = false;

        runner = FindFirstObjectByType<NetworkRunner>();

        cam = Camera.main;
    }

    // Raycast for testing, removed later when enemies are spawning the holes.
    private RaycastHit CastRay()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            Quaternion rot = Quaternion.Euler(0, hit.transform.rotation.y, 0);
        }
        return hit;
    }

    public void TrySpawn(RigInput input)
    {
        if (!input.buttons.IsSet(Input.Trigger)) return;

        // Used for testing, removed later
        RaycastHit ray = CastRay();

        if (!runner.IsServer)
        {
            RPCRequestSpawnHole(ray.point, ray.transform.localRotation, PlayerRef.None);
        }
        else
        {
            SpawnHole(ray.point, ray.transform.localRotation);
        }
    }

    [Rpc(InvokeLocal = false)]
    void RPCRequestSpawnHole(Vector3 pos, Quaternion rot, [RpcTarget] PlayerRef target)
    {
        if (!Runner.IsServer) return;
        SpawnHole(pos, rot);
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
            NetworkObject spawnedHole = runner.Spawn(holePrefabs[holeIndex], pos, rot);
            spawnedHole.transform.parent = parent.transform;
        }
        // Seperate holes not connected.
        else
        {
            NetworkObject spawnedHole = runner.Spawn(holePrefabs[holeIndex], pos, rot);
        }

        parent = null;
    }


}
