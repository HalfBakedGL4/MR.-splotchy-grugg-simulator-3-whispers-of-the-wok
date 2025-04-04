using Fusion;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_ItemSpawner : NetworkBehaviour
{
    [SerializeField] GameObject item;

    public void SpawnItem(InputInfo info)
    {
        if (!info.context.started) return;

        if(item != null)
            Runner.Spawn(item, transform.position, Quaternion.identity);
    }
}
