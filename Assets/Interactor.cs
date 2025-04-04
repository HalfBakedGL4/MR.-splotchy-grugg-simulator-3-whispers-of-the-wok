using Extentions.Networking;
using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_Interactor : NearFarInteractor
{
    protected override async void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.TryGetComponent(out NetworkObject networkObject))
        {
            await Shared.GetStateAuthority(networkObject);
        }

        base.OnSelectEntered(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

    }
}
