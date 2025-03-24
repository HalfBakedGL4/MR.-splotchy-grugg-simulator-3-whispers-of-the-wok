using System;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class S_Ticket : MonoBehaviour
{
    [SerializeField] private InputActionProperty leftInputAction;
    [SerializeField] private InputActionProperty rightInputAction;


    [SerializeField] private TextMeshProUGUI ticketNumberText;
    
    [SerializeField] private Image orderImage;
    [SerializeField] private Image orderIngredients;
    [SerializeField] private Image orderTools;
    [SerializeField] private Image orderTutorial;

    [SerializeField] private GameObject page1;
    [SerializeField] private GameObject page2;
    private static int ticketNumber;
    
    private bool isHeld = false;
    private bool isLeft = false;
    
    public void InitTicket(Order order)
    {
        //Number the ticket
        ticketNumber++;
        ticketNumberText.text = $"#{ticketNumber}";
        
        // Put images on ticket
        orderImage.sprite = order.orderImage;
        orderIngredients.sprite = order.orderIngredients;
        orderTools.sprite = order.orderTools;
        orderTutorial.sprite = order.orderTutorial;
    }

    public void TicketHeld(SelectEnterEventArgs args)
    {
        isHeld = true;

        // Checks which hand is holding to make ui only moved by said hand
        isLeft = args.interactorObject.transform.name == "Left Controller";
    }

    public void TicketReleased(SelectExitEventArgs args)
    {
        isHeld = false;
    }


    private void Update()
    {
        var isCorrectHand = (isLeft && leftInputAction.action.ReadValue<float>() > 0) || 
                            (!isHeld && rightInputAction.action.ReadValue<float>() > 0);
        if (isHeld && isCorrectHand)
        {
            SwapPage();
        }
    }

    private void SwapPage()
    {
        page1.SetActive(!page1.activeSelf);
        page2.SetActive(!page2.activeSelf);
    }
}
