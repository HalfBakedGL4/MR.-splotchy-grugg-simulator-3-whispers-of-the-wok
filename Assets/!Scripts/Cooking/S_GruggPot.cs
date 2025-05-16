using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_GruggPot : NetworkBehaviour, IButtonObject, IToggle
{
    [SerializeField] private GameObject gruggJuiceCollider;
    [SerializeField] private XRBaseInteractable interactable;
    [Networked] private bool isTurnedOn { get; set; }

    private bool isActive = false;
    private void Start()
    {
        gruggJuiceCollider.SetActive(false);
    }

    // Whenever button is pressed, start spewing grugg
    public void OnButtonPressed()
    {
        if (!isActive)
            StartCoroutine(ApplyGruggWindow());
    }

    private IEnumerator ApplyGruggWindow()
    {
        // GruggJuice Trigger is made visible and can add the Grugg to dish
        isActive = true;
        gruggJuiceCollider.SetActive(true);
        // Should add VFX and SFX here
        yield return new WaitForSeconds(2f);
        // Hides the Trigger for the GruggJuice
        isActive = false;
        gruggJuiceCollider.SetActive(false);
    }

    public void SetApplicationActive(bool toggle)
    {
        isTurnedOn = toggle;
        interactable.enabled = toggle;
    }
}
