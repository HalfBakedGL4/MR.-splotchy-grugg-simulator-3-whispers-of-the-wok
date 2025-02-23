using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_ButtonPushed : MonoBehaviour
{
    [SerializeField] S_Cooker cooker;
    private void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => ToggleButton());
    }

    private void ToggleButton()
    {
        cooker.InteractWithCooker();
    }
}
