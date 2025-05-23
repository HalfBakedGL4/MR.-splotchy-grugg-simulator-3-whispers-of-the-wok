using System;
using System.Collections.Generic;
using Fusion;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_PlateDispenser : NetworkBehaviour, IToggle
{
    
    [SerializeField] private Transform insertedPlateTransform;
    public event Action<Transform> OnPlateRemoved;
    
    List<GameObject> plates = new ();

    [Networked] private bool isTurnedOn { get; set; }

    public override void Spawned()
    {
        base.Spawned();

        ConnectToApplicationManager();
    }
    
    public void ConnectToApplicationManager()
    {
        if (S_ApplicationManager.Instance != null)
        {
            S_ApplicationManager.Instance.RegisterToggle(this);
        }
    }

    public void SpawnPlateWhenEnabled()
    {
        // Spawn New Plate
        if (HasStateAuthority)
        {
            RPC_OnPlateRemoved();
        }
    }
    

    public void InsertPlate(SelectEnterEventArgs args)
    {
        plates.Add(args.interactorObject.transform.gameObject);
        // Hides any plates that is added so they can't be picked up
        if (!isTurnedOn)
        {
            args.interactorObject.transform.gameObject.SetActive(false);
            return;
        }

        // Check if plate
        if (args.interactorObject.transform.TryGetComponent(out S_Plate plate))
        {
            // Save Plate
            var insertedPlate = plate.GetComponent<XRSocketInteractor>();
           
            // Disable the Plate's Socket
            insertedPlate.socketActive = false;
        }
    }
    

    public void PickUpPlate(SelectExitEventArgs args)
    {
        if (!isTurnedOn) {return;}

        // Check if plate
        if (args.interactorObject.transform.TryGetComponent(out S_Plate plate))
        {
            // Save Plate
            var insertedPlate = plate.GetComponent<XRSocketInteractor>();
           
            // Disable the Plate's Socket
            insertedPlate.socketActive = true;
        }
        
        // Spawn New Plate
        if (HasStateAuthority)
        {
            RPC_OnPlateRemoved();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_OnPlateRemoved()
    {
        OnPlateRemoved?.Invoke(insertedPlateTransform);
    }

    public void SetApplicationActive(bool toggle)
    {
        isTurnedOn = toggle;

        if (toggle && plates.Count == 0)
        {
            SpawnPlateWhenEnabled();
        }
        TogglePlateVisible(toggle);
    }

    private void TogglePlateVisible(bool toggle)
    {
        foreach (var plate in plates)
        {
            plate.SetActive(toggle);
        }
        print(name + " is turned on: " + toggle);
        ToggleMovement(toggle);

    }

    public void ToggleMovement(bool toggle)
    {
        // Not in use
    }
}
