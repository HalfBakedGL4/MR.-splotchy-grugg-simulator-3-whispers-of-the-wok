using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_GruggPot : NetworkBehaviour, IButtonObject, IToggle
{
    [SerializeField] private GameObject gruggJuiceCollider;
    [SerializeField] private XRBaseInteractable interactable;
    [Networked] private bool isTurnedOn { get; set; }

    [Networked] private bool isActive { get; set; }
    public override void Spawned()
    {
        base.Spawned();
        
        ConnectToApplicationManager();
        
        RPC_GruggColliderActive(false);
    }

    // Whenever button is pressed, start spewing grugg
    public void OnButtonPressed()
    {
        if (!isActive && isTurnedOn)
            StartCoroutine(ApplyGruggWindow());
    }
    
    private IEnumerator ApplyGruggWindow()
    {
        // GruggJuice Trigger is made visible and can add the Grugg to dish
        isActive = true;
        RPC_GruggColliderActive(true);
        // Should add VFX and SFX here
        yield return new WaitForSeconds(2f);
        // Hides the Trigger for the GruggJuice
        isActive = false;
        RPC_GruggColliderActive(false);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    private void RPC_GruggColliderActive(bool toggle)
    {
        gruggJuiceCollider.SetActive(toggle);
    }
    
    public void SetApplicationActive(bool toggle)
    {
        isTurnedOn = toggle;
        
        print(name + " is turned on: " + toggle);

        RPC_ToggleMovement(toggle);

    }

    private XRGrabInteractable _grabInteractable;
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_ToggleMovement(bool toggle)
    {
        if (_grabInteractable == null)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }
        interactable.enabled = toggle;

        
        // Is opposite of toggle because it needs to be on when everything is off
        _grabInteractable.enabled = !toggle;
    }

    public void ConnectToApplicationManager()
    {
        if (S_ApplicationManager.Instance != null)
        {
            S_ApplicationManager.Instance.RegisterToggle(this);
        }
    }
}
