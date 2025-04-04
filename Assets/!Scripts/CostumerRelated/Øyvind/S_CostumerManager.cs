using System;
using Fusion;
using UnityEngine;

public class S_CostumerManager : NetworkBehaviour
{
    [SerializeField] private S_CostumerOrder costumerOrder;
    [SerializeField] private S_FindWallToBreak findWallToBreak;
    
    bool isLocal => Object && Object.HasStateAuthority;

    private void Awake()
    {
        //if (!isLocal) enabled = false;

    }

    private void Start()
    {

        

    }

    public override void Spawned()
    {
        base.Spawned();
        
        costumerOrder.OrderFood();
        
        InvokeRepeating(nameof(Debuggins), 0f, 10f);
    }

    private void Update()
    {
        Debug.Log(isLocal) ;
    }

    void Debuggins()
    {
        findWallToBreak.Debuggings();
    }
}
