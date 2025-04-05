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

        await Shared.GainStateAuthority(networkObject);

        base.OnSelectEntered(args);
    }
}
