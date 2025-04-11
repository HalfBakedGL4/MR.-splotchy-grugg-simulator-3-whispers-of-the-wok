using Fusion;
using UnityEngine;

public class S_SpawnNetworkObject : NetworkBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform[] SpawnPoints;
    
    public override void Spawned()
    {
        base.Spawned();
        foreach (var spawnPoint in SpawnPoints)
        {
            Runner.Spawn(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
