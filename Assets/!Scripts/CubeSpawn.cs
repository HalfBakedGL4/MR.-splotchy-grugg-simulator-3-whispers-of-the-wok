using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float startSpeed = 2;
    [SerializeField] private InputActionProperty inputAction;

    NetworkRunner runner;


    private void Start()
    {
        runner = FindFirstObjectByType<NetworkRunner>();
    }

    void Update()
    {
        if (inputAction.action.WasPressedThisFrame())
        {
            RPC_CreateCube();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    void RPC_CreateCube()
    {
        NetworkObject spawnedCub = runner.Spawn(cubePrefab, transform.position, transform.rotation);
        Rigidbody cubeRigidbody = spawnedCub.GetComponent<Rigidbody>();
        cubeRigidbody.linearVelocity = transform.forward * startSpeed;
    }

}
