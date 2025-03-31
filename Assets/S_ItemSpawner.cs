using Fusion;
using UnityEngine;

public class S_ItemSpawner : NetworkBehaviour
{
    [SerializeField] GameObject item;
    bool isLocal => Object && Object.HasStateAuthority;

    public void SpawnItem()
    {
        if (!isLocal) return;
        if(item != null)
        Runner.Spawn(item, transform.position, Quaternion.identity);
    }
}
