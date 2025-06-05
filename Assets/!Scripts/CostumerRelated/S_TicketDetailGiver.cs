using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class S_TicketDetailGiver : NetworkBehaviour
{
    [SerializeField] private InputActionProperty leftInputAction;
    [SerializeField] private InputActionProperty rightInputAction;

    [SerializeField] private TextMeshProUGUI ticketNumberText;

    //[SerializeField] private Image orderImage;
    //[SerializeField] private Image orderIngredients;
    //[SerializeField] private Image orderTools;
    //[SerializeField] private Image orderTutorial;

    [SerializeField] private GameObject page1;
    [SerializeField] private GameObject page2;

    private bool IsLocal => Object && Object.HasStateAuthority;
    
    [Networked]
    private int TicketID { get; set; }
    
    private bool _isHeld = false;
    private bool _isLeft = false;
    private bool _swappedPage = false;
    
    public void InitTicket(int ticketID)
    {
        //Number the ticket
        if (IsLocal)
        {
            TicketID = ticketID;
        }
        
        // Put images on ticket
        //orderImage.sprite = order.orderImage;
        //orderIngredients.sprite = order.orderIngredients;
        //orderTools.sprite = order.orderTools;
        //orderTutorial.sprite = order.orderTutorial;
        
        
    }

    public int GetTicketID()
    {
        return TicketID;
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
    
    private void Update()
    {
        var isCorrectHand = ((_isLeft && leftInputAction.action.WasPerformedThisFrame()) || 
                             (!_isLeft && rightInputAction.action.WasPerformedThisFrame()));
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
