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
    
    
    [Header("Debugging, will remove later")]
    [SerializeField] private TMP_Text debugText;


    private bool isLocal => Object && Object.HasStateAuthority;
    
    public override void Spawned()
    {
        base.Spawned();

        if (isLocal)
        {
            //InvokeRepeating(nameof(Debuggings), 0f, 10f);
            
            costumerOrder.OrderFood();
        }
    }
    
    private void Debuggings()
    {
        // Find point on wall
        var wallTuple = FindAndStoreWallToBreak();

        debugText.text += " Making ";
        MakeHoleInWall(wallTuple);
        debugText.text += " hole, ";

        debugText.text += " Spawning ";
        SpawnCostumer(wallTuple);
        debugText.text += " costumer, ";
    }


    private (ARPlane, Vector3)  FindAndStoreWallToBreak()
    {       
        // Gets Tuple (AR Plane: wall, Vector 3: point)
        return findPointsOnWalls.GetRandomWallAndPoint();
    }

    private void MakeHoleInWall((ARPlane wall, Vector3 pointOnWall) wallTuple)
    {
        var holePos = wallTuple.pointOnWall + wallTuple.wall.transform.position;

        holeSpawner.SpawnHole(holePos, wallTuple.wall.transform.rotation);
    }
    
    private void SpawnCostumer( (ARPlane wall, Vector3 pointOnWall) wallTuple)
    {
        var costumerPos = wallTuple.pointOnWall + wallTuple.wall.transform.position;

        costumerSpawner.SpawnCostumer(costumerPos, wallTuple.wall);
    }

}
