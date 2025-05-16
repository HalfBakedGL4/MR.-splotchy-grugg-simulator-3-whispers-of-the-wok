using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_ApplicationManager : NetworkBehaviour
{

    private S_Cooker[] _cookers;

    private S_GruggPot[] _gruggPots;
    
    private S_FoodDispenser[] _foodDispensers;
    
    private S_PlateDispenser[] _plateDispensers;
    
    private List<XRBaseInteractable> _interactables = new ();
    
    private S_TrashCanManager _trashCanManager;

    public override void Spawned()
    {
        base.Spawned();
        
        _ = FindApplicationsInScene();
        
        
    }
    
    private async Task FindApplicationsInScene()
    {
        await Task.Yield();
        
        _cookers = FindObjectsByType<S_Cooker>(FindObjectsSortMode.None);

        _interactables = new List<XRBaseInteractable>();
        foreach (var cooker in _cookers)
        {
            XRBaseInteractable interactable = cooker.GetComponentInChildren<XRSimpleInteractable>();
            if (interactable)
            {
                _interactables.Add(interactable);
            }

            interactable = cooker.GetComponent<XRGrabInteractable>();
            if (interactable)
            {
                _interactables.Add(interactable);
            }

        }
        
        _gruggPots = FindObjectsByType<S_GruggPot>(FindObjectsSortMode.None);
        
        _foodDispensers = FindObjectsByType<S_FoodDispenser>(FindObjectsSortMode.None);
        
        _plateDispensers = FindObjectsByType<S_PlateDispenser>(FindObjectsSortMode.None);
        
        _trashCanManager = FindAnyObjectByType<S_TrashCanManager>();
    }

    private void DisableApplications()
    {
        foreach (var cooker in _cookers)
        {
            cooker.enabled = false;
        }

        foreach (var pot in _gruggPots)
        {
            pot.enabled = false;
        }

        foreach (var dispenser in _foodDispensers)
        {
            dispenser.enabled = false;
        }

        foreach (var dispenser in _plateDispensers)
        {
            dispenser.enabled = false;
        }

        _trashCanManager.enabled = false;
    }

    private void EnableApplications()
    {
        
    }
}
