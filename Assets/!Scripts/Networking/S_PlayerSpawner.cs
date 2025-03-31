using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using Networking.Shared;
using Extentions.Addressable;

public class S_PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [Header("Settings")]
    [SerializeField] bool spawnNetworkPlayer = true;

    [Header("References")]
    S_NetworkPlayer networkPlayerPrefab;
    S_LocalPlayer localPlayer;

    private Dictionary<PlayerRef, NetworkObject> _spawnedUsers = new Dictionary<PlayerRef, NetworkObject>();

    private async void Start()
    {
        if(networkPlayerPrefab == null)
        {
            networkPlayerPrefab = (await Addressable.LoadAsset<GameObject>(Addressable.addressables[0])).GetComponent<S_NetworkPlayer>();
        }

        if (localPlayer == null)
        {
            localPlayer = FindFirstObjectByType<S_LocalPlayer>();
        }
    }

#if UNITY_EDITOR
    private async void OnValidate()
    {
        if (networkPlayerPrefab == null)
        {
            networkPlayerPrefab = (await Addressable.LoadAsset<GameObject>(Addressable.addressables[0])).GetComponent<S_NetworkPlayer>();
        }

        if (localPlayer == null)
        {
            localPlayer = FindFirstObjectByType<S_LocalPlayer>();
        }
    }
#endif

    public void SpawnPlayer(PlayerRef player)
    {
        S_NetworkPlayer networkPlayer = Runner.Spawn(networkPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, inputAuthority: player, (runner, obj) => { });
        networkPlayer.SetLocalPlayer(localPlayer);

        _spawnedUsers.Add(player, networkPlayer.networkBehaviour);
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (!spawnNetworkPlayer) return;
        if (player != Runner.LocalPlayer) return;

        Debug.Log(player.ToString() + " joined the lobby");

        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (!spawnNetworkPlayer) return;

        if (_spawnedUsers.TryGetValue(player, out NetworkObject obj))
        {
            Runner.Despawn(obj);
            _spawnedUsers.Remove(player);
        }
    }
}
