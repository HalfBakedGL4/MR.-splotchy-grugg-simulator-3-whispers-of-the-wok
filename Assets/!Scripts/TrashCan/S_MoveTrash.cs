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
                    trashSockets[i].socketActive = true;

                if (_currentDestination[i] == 2)
                    trashSockets[i].socketActive = false;
            }
        }
    }
}