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
    
    private bool _isHeld = false;
    private bool _isLeft = false;
    private bool _swappedPage = false;
    
    private Order _currentOrder;
    private S_CostumerOrder _costumerOrder;
    public void InitTicket(Order order, S_CostumerOrder costumerOrder)
    {
        //Number the ticket
        ticketNumber++;
        ticketNumberText.text = $"#{ticketNumber}";
        
        // Put images on ticket
        orderImage.sprite = order.orderImage;
        orderIngredients.sprite = order.orderIngredients;
        orderTools.sprite = order.orderTools;
        orderTutorial.sprite = order.orderTutorial;
        
        // Save the order and costumer who ordered so that they can be found via the ticket later
        _currentOrder = order;
        _costumerOrder = costumerOrder;
    }

    public void TicketHeld(SelectEnterEventArgs args)
    {
        // When player picks up ticket, near far intertactor is interactorObject
        // To see if player has picked up ticket
        _isHeld = args.interactorObject.transform.name == "Near-Far Interactor";

        // Checks which hand is holding to make ui only moved by said hand
        _isLeft = args.interactorObject.transform.parent.name == "Left Controller";
    }

    public void TicketReleased(SelectExitEventArgs args)
    {
        // When ticket is released the ticket is loose again
        _isHeld = false;
    }

    public int GetTicketNumber()
    {
        return ticketNumber;
    }

    public (Order, S_CostumerOrder) GetOrderAndCostumer()
    {
        return (_currentOrder, _costumerOrder);
    }

    public Order GetOrder()
    {
        return _currentOrder;
    }

    public S_CostumerOrder GetCostumer()
    {
        return _costumerOrder;
    }

    private void Update()
    {
        var isCorrectHand = ((_isLeft && leftInputAction.action.WasPerformedThisFrame()) || 
                            (!_isLeft && rightInputAction.action.WasPerformedThisFrame()));
        print("Is input registered on the correct hand: " + isCorrectHand);
        if (_isHeld && isCorrectHand)
        {
            SwapPage();
            _swappedPage = true;
        }
    }

    private void SwapPage()
    {
        page1.SetActive(!page1.activeSelf);
        page2.SetActive(!page2.activeSelf);
    }
}
