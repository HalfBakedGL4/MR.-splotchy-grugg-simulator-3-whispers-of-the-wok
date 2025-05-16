using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class S_FryerActivater : NetworkBehaviour
{
    // Controller to activate and deactivate the fryer
    bool isLocal => Object && Object.HasStateAuthority;

    public void FryerPutInOil(SelectEnterEventArgs args)
    {
        if (!isLocal) { return; }
        
        if (args.interactableObject.transform.TryGetComponent(out S_Cooker fryer))
        {
            fryer.CheckIfFryerShouldStart();
        }
    }

    public void FryerLiftedOutOfOil(SelectExitEventArgs args)
    {
        if (!isLocal) { return; }

        if (args.interactableObject.transform.TryGetComponent(out S_Cooker fryer))
        {
               fryer.CantCook();
               fryer.InteractWithCooker();
        }
    }
}
