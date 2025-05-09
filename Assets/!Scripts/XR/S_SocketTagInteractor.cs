using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_SocketTagInteractor : XRSocketInteractor // Derives from built in code, but adds option to specify tag
{
    [SerializeField] private string targetTag;
    [SerializeField] private bool noTag;


    public override bool CanHover(IXRHoverInteractable interactable)
    {
        if(interactable.transform.TryGetComponent(out NetworkObject netwObj))
        {
            if (!netwObj.HasStateAuthority)
                return default;
        }
        if (noTag)
        {
            return base.CanHover(interactable) && true;
        }

        return base.CanHover(interactable) && interactable.transform.CompareTag(targetTag);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        if (interactable.transform.TryGetComponent(out NetworkObject netwObj))
        {
            if (!netwObj.HasStateAuthority)
                return default;
        }
        if (noTag)
        {
            return base.CanSelect(interactable) && true;
        }

        return base.CanSelect(interactable) && interactable.transform.CompareTag(targetTag);
    }
}
