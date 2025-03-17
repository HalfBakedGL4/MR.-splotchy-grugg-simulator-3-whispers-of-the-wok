using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_ButtonPushed : MonoBehaviour
{
    [SerializeField] private GameObject objectWithIButtonObject;
    private IButtonObject connectedToButton;
    private void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => ToggleButton());

        connectedToButton = GetComponent<IButtonObject>();
    }

    private void ToggleButton()
    {
        connectedToButton.OnButtonPressed();
    }
}
