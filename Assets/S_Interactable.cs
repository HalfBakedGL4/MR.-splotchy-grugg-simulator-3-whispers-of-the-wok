using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_Interactable : XRGrabInteractable
{
    [HorizontalLine]
    public UnityEvent Grabbed;

    protected override void Grab()
    {
        base.Grab();

        Debug.Log("[Interactable] Grabbed " + name);

        Grabbed?.Invoke();
    }
}
