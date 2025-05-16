using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class S_OpenDoor : NetworkBehaviour
{
    [SerializeField] private float startRotation = 0f;
    [SerializeField] private float endRotation = 180f;
    [SerializeField] private float rotationSpeed = 5f; // How fast the door opens/closes

    [Networked] private bool IsOpen { get; set; } = false;
    private float currentYRotation;

    public override void Spawned()
    {
        base.Spawned();
        // Initialize rotation
        currentYRotation = startRotation;
        SetRotationInstantly();
    }

    public void InteractWithDoor(SelectEnterEventArgs args)
    {

        IsOpen = !IsOpen;
        Debug.Log(IsOpen ? "opening door" : "closing door");
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        // Only update door on authoritative instance to prevent conflict with NetworkTransform
        if (!HasStateAuthority) return;

        float targetRotation = IsOpen ? endRotation : startRotation;
        currentYRotation = Mathf.LerpAngle(currentYRotation, targetRotation, Runner.DeltaTime * rotationSpeed);
        
        // Apply smoothed rotation
        transform.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);
    }

    private void SetRotationInstantly()
    {
        float targetRotation = IsOpen ? endRotation : startRotation;
        transform.localRotation = Quaternion.Euler(0f, targetRotation, 0f);
        currentYRotation = targetRotation;
    }
}