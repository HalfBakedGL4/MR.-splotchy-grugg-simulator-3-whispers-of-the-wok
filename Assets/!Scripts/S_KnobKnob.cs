using Extentions.Networking;
using Fusion;
using Unity.XRContent.Interaction;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_KnobKnob : S_Knob
{
    [SerializeField] private NetworkObject netObj;
    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        _=Shared.GainStateAuthority(netObj);
        base.OnSelectEntered(interactor);
    }
}
