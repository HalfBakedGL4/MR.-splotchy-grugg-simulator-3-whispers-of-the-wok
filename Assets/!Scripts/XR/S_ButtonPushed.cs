using NaughtyAttributes;
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
        if (objectWithIButtonObject == null) {return;}
        
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => ToggleButton());

        if (objectWithIButtonObject.TryGetComponent(out IButtonObject buttonObject))
        {
            connectedToButton = buttonObject;
        }
        else
        {
            Debug.LogError("Did not find IButtonObject on " + objectWithIButtonObject.name);
        }
    }
    [Button]
    private void ToggleButton()
    {
        connectedToButton.OnButtonPressed();
    }
}
