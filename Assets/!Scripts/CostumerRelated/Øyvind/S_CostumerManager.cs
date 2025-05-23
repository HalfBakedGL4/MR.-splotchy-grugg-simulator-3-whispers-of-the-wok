using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_CostumerManager : NetworkBehaviour, IToggle
{
    [Header("Scripts")]
    [SerializeField] private S_CostumerOrder costumerOrder;
    [SerializeField] private S_CostumerSpawner costumerSpawner;
    [SerializeField] private S_FindPointsOnWalls findPointsOnWalls;
    [SerializeField] private S_HoleSpawner holeSpawner;

    private bool isLocal => Object && Object.HasStateAuthority;
    [Networked] private bool isTurnedOn { get; set; }
        
  
    public override void Spawned()
    {
        base.Spawned();
        
        ConnectToApplicationManager();

        if (isLocal)
        {
            InvokeRepeating(nameof(Debuggings), 10f, 45f);
        }
    }
    
    public void ConnectToApplicationManager()
    {
        if (S_ApplicationManager.Instance != null)
        {
            S_ApplicationManager.Instance.RegisterToggle(this);
        }
    }
    
    private void Debuggings()
    {
        if (!isTurnedOn) { return; }
        
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
    
    public void SetApplicationActive(bool toggle)
    {
        isTurnedOn = toggle;
        print(name + " is turned on: " + toggle);

        ToggleMovement(toggle);

    }

    public void ToggleMovement(bool toggle)
    {
        // Doesn't need to be grabbed
    }


}
