using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_CostumerOrder : MonoBehaviour
{
    S_OrderWindow orderWindow;
    
    [Header("What Dishes the costumer can order")]
    [SerializeField] private List<Dish> canOrder = new ();

    private S_Ticket costumerTicket;
    private void Start()
    {
        // Find the window to place ticket
        orderWindow = FindAnyObjectByType<S_OrderWindow>();

        // TODO: Remove when implemented target
        OrderFood();
    }
    
    // The costumer would order food when they approach the window
    public void OrderFood()
    {
        var order = canOrder[Random.Range(0, canOrder.Count)];
        
        // Ticket is returned to the costumer so the costumer know which ticket they own
        // And costumer is given to the ticket so the ticket can reference the costumer
        costumerTicket = orderWindow.MakeOrder(order, this);
    }

    public void ReceiveDish()
    {
        // Check if correct Dish
        
        // Change anger based on Dish performance
        
        // Removes the ticket from the window
        RemoveTicket();
    }

    private void RemoveTicket()
    {
        orderWindow.RemoveTicket(costumerTicket);
    }
    
}
