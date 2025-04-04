using Extentions.Networking;
using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_Interactor : NearFarInteractor
{

    protected override async void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("[interactor] interact");
        if (!args.interactableObject.transform.TryGetComponent(out NetworkObject networkObject)) return;

        if (!await Shared.GetStateAuthority(networkObject)) return;

        Debug.Log("[interactor] state auth: " + networkObject.HasStateAuthority);

        base.OnSelectEntered(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

    }
}
