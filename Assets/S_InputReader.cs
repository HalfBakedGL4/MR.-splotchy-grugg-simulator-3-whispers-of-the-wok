using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class S_InputReader : MonoBehaviour, InputSystem_Actions.IRightControllerActions
{
    [HideInInspector] public InputSystem_Actions playerInput;

    private void OnEnable()
    {
        playerInput = new InputSystem_Actions();
        playerInput.RightController.SetCallbacks(this);

        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    public UnityEvent<InputAction.CallbackContext> RightTrigger;
    public void OnTrigger(InputAction.CallbackContext context)
    {
        Debug.Log("Trigger");
        RightTrigger?.Invoke(context);
    }
}
