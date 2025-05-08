using System;
using Fusion;
using UnityEngine;

public class S_TrashCanManager : NetworkBehaviour
{
    [SerializeField] private S_DestroyTrash destroyTrash;
    [SerializeField] private S_MoveTrash moveTrash;
    
    
    public override void Spawned()
    {
        base.Spawned();
        
        destroyTrash.enabled = true;
        moveTrash.enabled = true;
    }
}
