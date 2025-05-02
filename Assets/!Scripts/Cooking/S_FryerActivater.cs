using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class S_FryerActivater : MonoBehaviour
{
    // Controller to activate and deactivate the fryer

    public void FryerPutInOil(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.TryGetComponent(out S_Cooker fryer))
        {
            fryer.CheckIfFryerShouldStart(fryer);
        }
    }

    public void FryerLiftedOutOfOil(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.TryGetComponent(out S_Cooker fryer))
        {
               fryer.CantCook();
               fryer.InteractWithCooker();
        }
    }
}
