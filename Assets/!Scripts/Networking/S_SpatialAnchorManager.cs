using System;
using UnityEngine;
using Meta.XR;
using Fusion;

public class S_SpatialAnchorManager : NetworkBehaviour
{
    public Guid groupUuid;

    void Start()
    {
        if (Runner.IsServer)
        {
            // Create a new group UUID for the session
            groupUuid = Guid.NewGuid();
            ShareGroupUuidToParticipants(groupUuid);  // Propagate UUID to all clients
        }
    }

    private void ShareGroupUuidToParticipants(Guid groupUuid)
    {
        RpcShareGroupUuid(groupUuid);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RpcShareGroupUuid(Guid groupUuid)
    {
        this.groupUuid = groupUuid;
        Debug.Log($"Received Group UUID: {groupUuid}");
    }
}

