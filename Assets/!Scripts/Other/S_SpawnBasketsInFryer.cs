using Fusion;
using UnityEngine;

public class S_SpawnBasketsInFryer : NetworkBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform[] SpawnPoints;

    bool isLocal => Object && Object.HasStateAuthority;

    public override void Spawned()
    {
        base.Spawned();
        if (isLocal)
        {
            foreach (var spawnPoint in SpawnPoints)
            {
                Runner.Spawn(prefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
