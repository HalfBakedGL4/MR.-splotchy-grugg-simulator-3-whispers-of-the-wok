using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class S_OpenDoor : MonoBehaviour
{
    [SerializeField] private float startRotation;
    [SerializeField] private float endRotation;
    
    private bool isOpen = false;
    
    public void InteractWithDoor(SelectEnterEventArgs args)
    {
        if (isOpen)
        {
            transform.localRotation = Quaternion.Euler(transform.rotation.x, startRotation, transform.rotation.z);
            print("closing door");
        }
        else
        {
            transform.localRotation = Quaternion.Euler(transform.rotation.x, endRotation, transform.rotation.z);
            print("opening door");

        }
        isOpen = !isOpen;
    }
}
