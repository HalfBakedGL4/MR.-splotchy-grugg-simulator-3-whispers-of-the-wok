using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;

public class S_PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    public NetworkObject networkPlayerPrefab;

    private Dictionary<PlayerRef, NetworkObject> _spawnedUsers = new Dictionary<PlayerRef, NetworkObject>();


    public void SpawnPlayer(PlayerRef player)
    {
        NetworkObject networkPlayer = Runner.Spawn(networkPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, inputAuthority: player, (runner, obj) => { });

        _spawnedUsers.Add(player, networkPlayer);
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player != Runner.LocalPlayer) return;

        Debug.Log(player.ToString() + " joined the lobby");

        //if(player == runner.LocalPlayer)
        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (_spawnedUsers.TryGetValue(player, out NetworkObject obj))
        {
            Runner.Despawn(obj);
            _spawnedUsers.Remove(player);
        }
    }
}
