using Extentions.Networking;
using Fusion;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_Interactor : NearFarInteractor
{
    //protected override async void OnHoverEntered(HoverEnterEventArgs args)
    //{
    //    args.interactableObject.transform.TryGetComponent(out NetworkObject networkObject);
    //    await GainStateAuthority(networkObject);

    //    base.OnHoverEntered(args);
    //}
    protected override async void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!args.interactableObject.transform.TryGetComponent(out NetworkObject networkObject)) return;

        await GainStateAuthority(networkObject);

        base.OnSelectEntered(args);
    }

    async Task GainStateAuthority(NetworkObject networkObject)
    {
        if (networkObject.HasStateAuthority)
        {
            Debug.Log("[interactor] already has state authority");
        }
        Debug.Log("[interactor] interact");

        if (!await Shared.GetStateAuthority(networkObject))
        {
            Debug.LogError("[interactor] failed to gain state authority");
        }

        Debug.Log("[interactor] state auth: " + networkObject.HasStateAuthority);
    }
}
