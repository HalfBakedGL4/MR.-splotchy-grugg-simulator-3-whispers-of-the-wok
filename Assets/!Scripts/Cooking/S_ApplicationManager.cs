using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_ApplicationManager : NetworkBehaviour
{

    // private S_Cooker[] _cookers;
    // private S_GruggPot[] _gruggPots;
    // private S_FoodDispenser[] _foodDispensers;
    // private S_PlateDispenser[] _plateDispensers;
    // private S_TrashCanManager _trashCanManager;

    private IToggle[] _toggles;
    public override void Spawned()
    {
        base.Spawned();
        
        _ = FindApplicationsInScene();
        
        
    }
    
    private async Task FindApplicationsInScene()
    {
        await Task.Yield();
        
        // _cookers = FindObjectsByType<S_Cooker>(FindObjectsSortMode.None);
        // _gruggPots = FindObjectsByType<S_GruggPot>(FindObjectsSortMode.None);
        // _foodDispensers = FindObjectsByType<S_FoodDispenser>(FindObjectsSortMode.None);
        // _plateDispensers = FindObjectsByType<S_PlateDispenser>(FindObjectsSortMode.None);
        // _trashCanManager = FindAnyObjectByType<S_TrashCanManager>();

        _toggles = (IToggle[])FindObjectsByType<NetworkBehaviour>(FindObjectsSortMode.None).OfType<IToggle>();
    }

    private void DisableApplications()
    {
        foreach (var toggle in _toggles)
        {
            toggle.SetApplicationActive(false);
        }
        
    }

    private void EnableApplications()
    {
        foreach (var toggle in _toggles)
        {
            toggle.SetApplicationActive(false);
        }
        
    }
}
