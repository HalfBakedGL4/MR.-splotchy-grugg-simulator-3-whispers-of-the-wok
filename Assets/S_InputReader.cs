using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{

    public struct InputInfo
    {
        public InputAction.CallbackContext context;

        public InputInfo(InputAction.CallbackContext context)
        {
            this.context = context;
        }

        public static implicit operator InputInfo(InputInfo<Vector2> v)
        {
            throw new NotImplementedException();
        }
    }
    public struct InputInfo<T> where T : struct
    {
        public InputAction.CallbackContext context;
        public T value => context.ReadValue<T>();

        public InputInfo(InputAction.CallbackContext context)
        {
            this.context = context;
        }
    }

    public class S_InputReader : MonoBehaviour, InputSystem_Actions.IRightControllerActions, InputSystem_Actions.ILeftControllerActions
    {
        [HideInInspector] public InputSystem_Actions playerInput;

        private void OnEnable()
        {
            playerInput = new InputSystem_Actions();
            playerInput.RightController.SetCallbacks(this);
            playerInput.LeftController.SetCallbacks(this);

            playerInput.Enable();
        }
        private void OnDisable()
        {
            playerInput.Disable();
        }

        #region Right Hand

        public UnityEvent<InputInfo> RightJoystick;
        public void OnR_Joystick(InputAction.CallbackContext context)
        {
            InputInfo info = new InputInfo<Vector2>(context);
            RightJoystick?.Invoke(info);
        }

        public UnityEvent<InputInfo> RightTrigger;
        public void OnR_Trigger(InputAction.CallbackContext context)
        {
            InputInfo info = new InputInfo(context);
            RightTrigger?.Invoke(info);
        }

        public UnityEvent<InputInfo> RightGrip;
        public void OnR_Grip(InputAction.CallbackContext context)
        {
            InputInfo info = new InputInfo(context);
            RightGrip?.Invoke(info);
        }

        public UnityEvent<InputInfo> RightA;
        public void OnR_A(InputAction.CallbackContext context)
        {
            InputInfo info = new InputInfo(context);
            RightA?.Invoke(info);
        }

        public UnityEvent<InputInfo> RightB;
        public void OnR_B(InputAction.CallbackContext context)
        {
            InputInfo info = new InputInfo(context);
            RightB?.Invoke(info);
        }

        #endregion

        [HorizontalLine(color: EColor.White)]
        [Space]

        #region Left Hand

        public UnityEvent<InputInfo> LeftJoystick;
        public void OnL_Joystick(InputAction.CallbackContext context)
        {
            InputInfo info = new InputInfo<Vector2>(context);
            LeftJoystick?.Invoke(info);
        }

        public UnityEvent<InputInfo> LeftTrigger;
        public void OnL_Trigger(InputAction.CallbackContext context)
        {
            InputInfo info = new InputInfo(context);
            LeftTrigger?.Invoke(info);
        }

        public UnityEvent<InputInfo> LeftGrip;
        public void OnL_Grip(InputAction.CallbackContext context)
        {
            InputInfo info = new InputInfo(context);
            LeftGrip?.Invoke(info);
        }

        public UnityEvent<InputInfo> LeftX;
        public void OnL_X(InputAction.CallbackContext context)
        {
            InputInfo info = new InputInfo(context);
            LeftX?.Invoke(info);
        }

        public UnityEvent<InputInfo> LeftY;
        public void OnL_Y(InputAction.CallbackContext context)
        {
            InputInfo info = new InputInfo(context);
            LeftY?.Invoke(info);
        }

        #endregion
    }
}
