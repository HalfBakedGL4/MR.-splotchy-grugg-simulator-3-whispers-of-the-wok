using Fusion;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float startSpeed = 2;
    [SerializeField] private InputActionProperty inputAction;

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
            Debug.Log("DETECTED: Cube Spawn button click");
            RPC_CreateCube();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    void RPC_CreateCube()
    {
        try
        {
            NetworkObject spawnedCub = runner.Spawn(cubePrefab, transform.position, transform.rotation, inputAuthority: runner.LocalPlayer);
            Rigidbody cubeRigidbody = spawnedCub.GetComponent<Rigidbody>();
            cubeRigidbody.linearVelocity = transform.forward * startSpeed;
            Debug.Log("CUBESPAWN: Cube Spawned");
        } catch (Exception e)
        {
            Debug.LogError("CUBESPAWN:" + e.Message);
        }
    }

}
