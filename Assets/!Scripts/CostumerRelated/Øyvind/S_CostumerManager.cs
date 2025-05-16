using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class S_CostumerManager : NetworkBehaviour
{
    [Header("Scripts")]
    [SerializeField] private S_CostumerOrder costumerOrder;
    [SerializeField] private S_CostumerSpawner costumerSpawner;
    [SerializeField] private S_FindPointsOnWalls findPointsOnWalls;
    [SerializeField] private S_HoleSpawner holeSpawner;

    private bool isLocal => Object && Object.HasStateAuthority;
    
    public override void Spawned()
    {
        base.Spawned();

        if (isLocal)
        {
            InvokeRepeating(nameof(Debuggings), 10f, 45f);
        }
    }
    
    private void Debuggings()
    {
        // Find point on wall
        var wallTuple = FindAndStoreWallToBreak();

        costumerOrder.OrderFood();
        

        SpawnCostumer(wallTuple);
    }


    private (ARPlane, Vector3)  FindAndStoreWallToBreak()
    {       
        // Gets Tuple (AR Plane: wall, Vector 3: point)
        return findPointsOnWalls.GetRandomWallAndPoint();
    }
    
    private void SpawnCostumer( (ARPlane wall, Vector3 pointOnWall) wallTuple)
    {
        var costumerPos = wallTuple.pointOnWall + wallTuple.wall.transform.position;

        costumerSpawner.SpawnCostumer(costumerPos, wallTuple.wall);

    }

}
