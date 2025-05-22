using System;
using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_MoveTrash : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform[] trashDestinations;
    [SerializeField] private XRSocketInteractor[] trashSockets;

    private int[] _currentDestination;

    public override void Spawned()
    {
        // Only StateAuthority (usually Host) controls movement
        if (!HasStateAuthority)
        {
            // Disable logic on non-authority clients
            enabled = false;
            return;
        }

        _currentDestination = new int[trashSockets.Length];

        for (int i = 0; i < _currentDestination.Length; i++)
        {
            _currentDestination[i] = 0;
        }

        PlaceSockets();
    }

    private void PlaceSockets()
    {
        var beltLength = Vector3.Distance(trashDestinations[0].position, trashDestinations[1].position) 
                         + Vector3.Distance(trashDestinations[1].position, trashDestinations[2].position)
                         + Vector3.Distance(trashDestinations[2].position, trashDestinations[3].position)
                         + Vector3.Distance(trashDestinations[3].position, trashDestinations[0].position);
        
        var offset = beltLength / trashSockets.Length;
        
        for (int i = 0; i < trashSockets.Length; i++)
        {
            trashSockets[i].transform.position = new Vector3(trashSockets[0].transform.position.x,
                trashSockets[0].transform.position.y, trashSockets[0].transform.position.z + offset * i);
        }
    }

    public override void FixedUpdateNetwork()
    {
        MoveTrashTowardsDestination();
    }

    private void MoveTrashTowardsDestination()
    {
        for (int i = 0; i < trashSockets.Length; i++)
        {
            Transform socketTransform = trashSockets[i].transform;
            Vector3 target = trashDestinations[_currentDestination[i]].position;

            socketTransform.position = Vector3.MoveTowards(socketTransform.position, target, moveSpeed * Runner.DeltaTime);

            if (Vector3.Distance(socketTransform.position, target) < 0.1f)
            {
                _currentDestination[i] = (_currentDestination[i] + 1) % trashDestinations.Length;

                if (_currentDestination[i] == 1)
                    RPC_ToggleSocketActive(i, true);


                if (_currentDestination[i] == 2)
                    RPC_ToggleSocketActive(i, false);
            }
        }
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_ToggleSocketActive(int i, bool toggle)
    {
        trashSockets[i].socketActive = toggle;
    }
}