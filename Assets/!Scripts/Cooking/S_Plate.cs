using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class S_Plate : MonoBehaviour
{
    public S_DishStatus dishStatus;

    GameObject food;

    public void PlateFood(SelectEnterEventArgs args)
    {
        
        dishStatus = args.interactableObject.transform.GetComponent<S_DishStatus>();
        food = args.interactableObject.transform.gameObject;
    }

    public void UnplateFood(SelectExitEventArgs args) 
    {
        
    }

    private void OnDestroy()
    {
        Destroy(food);
    }
}
