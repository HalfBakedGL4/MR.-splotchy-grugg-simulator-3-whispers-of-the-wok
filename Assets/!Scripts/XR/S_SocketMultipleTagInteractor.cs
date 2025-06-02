using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_SocketMultipleTagInteractor : XRSocketInteractor // Derives from built in code, but adds option to specify multiple tags
{
    [SerializeField] private string[] targetTags;


    public override bool CanHover(IXRHoverInteractable interactable)
    {
        if(interactable.transform.TryGetComponent(out NetworkObject netwObj))
        {
            if (!netwObj.HasStateAuthority)
                return default;
        }
        return base.CanHover(interactable) && CheckAllTags(interactable);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        if (interactable.transform.TryGetComponent(out NetworkObject netwObj))
        {
            if (!netwObj.HasStateAuthority)
                return default;
        }

        return base.CanSelect(interactable) && CheckAllTags(interactable);
    }

    private bool CheckAllTags(IXRInteractable interactable)
    {
        foreach (var targetTag in targetTags)
        {
            if (interactable.transform.CompareTag(targetTag))
            {
                return true;
            }
        }

        return false;
    }
}
