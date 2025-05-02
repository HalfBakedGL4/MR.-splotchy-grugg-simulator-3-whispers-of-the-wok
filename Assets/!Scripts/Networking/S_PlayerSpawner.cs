using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using Networking.Shared;
using Extentions.Addressable;
using System.Threading.Tasks;

public class S_PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [Header("Settings")]
    [SerializeField] bool spawnNetworkPlayer = true;

    [Header("References")]
    [SerializeField, ReadOnly] S_NetworkPlayer networkPlayerPrefab;
    S_LocalPlayer localPlayer;

    public static Dictionary<PlayerRef, NetworkObject> _spawnedUsers = new Dictionary<PlayerRef, NetworkObject>();

#if UNITY_EDITOR
    private async void OnValidate()
    {
        if (networkPlayerPrefab == null)
        {
            networkPlayerPrefab = await Addressable.LoadAsset<S_NetworkPlayer>(AddressableAsset.SharedNetworkPlayer);
        }

        if (localPlayer == null)
        {
            localPlayer = FindFirstObjectByType<S_LocalPlayer>();
        }
    }
#endif

    public async Task SpawnPlayer(PlayerRef player)
    {
        if (networkPlayerPrefab == null)
            networkPlayerPrefab = await Addressable.LoadAsset<S_NetworkPlayer>(AddressableAsset.SharedNetworkPlayer);
        

        if (localPlayer == null)
            localPlayer = FindFirstObjectByType<S_LocalPlayer>();
        

        S_NetworkPlayer networkPlayer = Runner.Spawn(networkPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, inputAuthority: player, (runner, obj) => { });
        networkPlayer.SetLocalPlayer(localPlayer);

        _spawnedUsers.Add(player, networkPlayer.networkBehaviour);
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (!spawnNetworkPlayer) return;
        if (player != Runner.LocalPlayer) return;

        Debug.Log(player.ToString() + " joined the lobby");

        _ = SpawnPlayer(player);
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
