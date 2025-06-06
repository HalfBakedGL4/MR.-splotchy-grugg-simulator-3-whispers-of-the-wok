using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_CostumerManager : NetworkBehaviour, IToggle
{
    [Header("Scripts")]
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
        
        if (GameState.Ongoing != S_GameManager.CurrentGameState) return;
        // Find point on wall
        var wallTuple = FindAndStoreWallToBreak();

        

        var costumerOrderInstanse = SpawnCostumer(wallTuple);
    }


    private (ARPlane, Vector3)  FindAndStoreWallToBreak()
    {       
        // Gets Tuple (AR Plane: wall, Vector 3: point)
        return findPointsOnWalls.GetRandomWallAndPoint();
    }
    
    private S_CostumerOrder SpawnCostumer( (ARPlane wall, Vector3 pointOnWall) wallTuple)
    {
        var costumerPos = wallTuple.pointOnWall + wallTuple.wall.transform.position;

        return costumerSpawner.SpawnCostumer(costumerPos, wallTuple.wall);

    }
    
    public void SetApplicationActive(bool toggle)
    {
        isTurnedOn = toggle;
        print(name + " is turned on: " + toggle);


    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_ToggleMovement(bool toggle)
    {
        // Doesn't need to be grabbed
    }


}
