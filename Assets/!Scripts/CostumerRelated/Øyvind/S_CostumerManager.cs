using System;
using Fusion;
using UnityEngine;

public class S_CostumerManager : NetworkBehaviour
{
    [SerializeField] private S_CostumerOrder costumerOrder;
    [SerializeField] private S_FindWallToBreak findWallToBreak;
    
    bool isLocal => Object && Object.HasStateAuthority;
    
    public override void Spawned()
    {
        base.Spawned();

        if (isLocal)
        {
            InvokeRepeating(nameof(Debuggins), 0f, 4f);

            //costumerOrder.OrderFood();
        }
        
    }
    
    void Debuggins()
    {
        findWallToBreak.Debuggings();
    }
}
