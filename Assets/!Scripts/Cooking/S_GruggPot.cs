using System.Collections;
using UnityEngine;

public class S_GruggPot : MonoBehaviour, IButtonObject
{
    [SerializeField] private GameObject gruggJuiceCollider;

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
}
