using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon;
using Fusion;

public class HoleSpawner : NetworkBehaviour
{
    Camera cam => Camera.main; // Only used for raycast, remove later.
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
    }

    void Update()
    {
        if (inputAction.action.WasPressedThisFrame())
        {
            Debug.Log("pressed Button");
            CastRay(out RaycastHit hit);
            RPC_CreatePortal(hit.point, hit.transform.rotation);
        }
    }

    //[Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
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
            NetworkObject spawnedHole = runner.Spawn(holePrefabs[holeIndex], transform.position, transform.rotation);
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
