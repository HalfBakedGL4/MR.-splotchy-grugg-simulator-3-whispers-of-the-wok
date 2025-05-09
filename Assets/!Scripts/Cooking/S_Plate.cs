using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class S_Plate : MonoBehaviour
{
    public S_DishStatus dishStatus;

    public void PlateFood(SelectEnterEventArgs args)
    {
        dishStatus = args.interactableObject.transform.GetComponent<S_DishStatus>();
    }
}
