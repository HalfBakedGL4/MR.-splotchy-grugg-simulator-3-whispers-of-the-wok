using System;
using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_PlateDispenser : NetworkBehaviour
{
    private XRSocketInteractor insertedPlate;
    
    [SerializeField] private Transform insertedPlateTransform;
    public event Action<Transform> OnPlateRemoved;


    public void FirstTimeSpawnPlate()
    {
        // Spawn New Plate
        if (HasStateAuthority)
        {
            RPC_OnPlateRemoved();
        }
    }
    

    public void InsertPlate(SelectEnterEventArgs args)
    {
        // Check if plate
        if (args.interactorObject.transform.TryGetComponent(out S_Plate plate))
        {
            // Save Plate
            insertedPlate = plate.GetComponent<XRSocketInteractor>();
           
            // Disable the Plate's Socket
            insertedPlate.socketActive = false;
        }
    }
    

    public void PickUpPlate(SelectExitEventArgs args)
    {
        // Enable the Plate's Socket
        insertedPlate.socketActive = true;
        
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
}
