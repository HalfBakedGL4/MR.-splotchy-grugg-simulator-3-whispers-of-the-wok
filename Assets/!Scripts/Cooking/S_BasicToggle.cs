using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_BasicToggle : NetworkBehaviour, IToggle
{
    private XRGrabInteractable _grabInteractable;

    [Networked] private bool isTurnedOn { get; set; }

    public void SetApplicationActive(bool toggle)
    {
        isTurnedOn = toggle;
        
        print(name + " is turned on: " + toggle);

        RPC_ToggleMovement(toggle);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_ToggleMovement(bool toggle)
    {
        if (_grabInteractable == null)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }
        
        // Is opposite of toggle because it needs to be on when everything is off
        _grabInteractable.enabled = !toggle;
    }

    public override void Spawned()
    {
        base.Spawned();
        
        ConnectToApplicationManager();
    }

    public void ConnectToApplicationManager()
    {
        if (S_ApplicationManager.Instance != null)
        {
            S_ApplicationManager.Instance.RegisterToggle(this);
        }
    }
}