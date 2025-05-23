using Fusion;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;

public interface IToggle
{
    void SetApplicationActive(bool toggle);
    
    void ToggleMovement(bool toggle);
    
    void ConnectToApplicationManager();
}
