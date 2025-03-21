using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ticket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ticketNumberText;
    
    [SerializeField] private Image orderImage;
    [SerializeField] private Image orderIngredients;
    [SerializeField] private Image orderTools;
    [SerializeField] private Image orderTutorial;
    
    private static int ticketNumber;
    
    
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
}
