using Fusion;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_CostumerOrder : NetworkBehaviour
{
    S_OrderWindow orderWindow;
    
    [Header("What Dishes the costumer can order")]
    [SerializeField] private List<DishType> canOrder = new ();

    private S_Ticket costumerTicket;
    
    private DishType orderedDish;
    private void Start()
    {
        // Find the window to place ticket
        orderWindow = FindAnyObjectByType<S_OrderWindow>();
        if (orderWindow == null)
            Debug.LogError("Could not find OrderWindow");
    }
    
    // The costumer would order food when they approach the window
    public S_Ticket OrderFood()
    {
        if (orderWindow == null)
            orderWindow = FindAnyObjectByType<S_OrderWindow>();

        orderedDish = canOrder[Random.Range(0, canOrder.Count)];
        
        // Ticket is returned to the costumer so the costumer know which ticket they own
        // And costumer is given to the ticket so the ticket can reference the costumer
        costumerTicket = orderWindow.MakeOrder(orderedDish, this);
        return costumerTicket;
    }

    // The costumer receive their order and will calculate their enjoyment
    // If not happy, this will affect the anger bar
    public void ReceiveDish(S_DishStatus dish)  //TODO: Add all the correct values to anger value
    {
        // Get the dish status
        var dishStatus = dish.GetDishStatus();
        
        // The local save for anger meter to send to the manager at the end
        var angerValue = 0.0f;
        
        if (orderedDish == dishStatus.typeOfDish) // Check if correct Dish
        {
            // IS correct dish
            Debug.Log("Correct Dish");
        }
        else
        {
            // IS wrong dish
            Debug.Log("Incorrect Dish");
            angerValue += 20;
            // Increase anger meter
        }
        
        switch (dishStatus.dishStatus) // Change anger meter based on Dish performance
        {
            case DishStatus.UnCooked:
                // Increase anger meter by a lot
                angerValue += 30;
                break;
            case DishStatus.UnderCooked:
                // Increase anger meter 
                angerValue += 10;
                break;
            case DishStatus.Cooked:
                // Is correct dish
                // Maybe reduce anger meter
                break;
            case DishStatus.OverCooked:
                // Increase anger meter
                angerValue += 10;
                break;
            case DishStatus.Burnt:
                // Increase anger meter by a lot
                angerValue += 30;
                break;
        }

        Debug.Log("Dish is: " + dishStatus.dishStatus);
        
        if (dishStatus.grugged)
        {
            Debug.Log("Dish is grugged");
            angerValue -= 10;
        }
        else
        {
            Debug.Log("Dish is not grugged");
            angerValue += 50;
        }       
        // Removes the ticket from the window
        RemoveTicket();
        // Send angerValue to anger manager
        Debug.Log("Anger Value: " + angerValue);
    }

    private void RemoveTicket()
    {
        Debug.Log(orderWindow);
        S_OrderWindow.RemoveTicket(costumerTicket);
    }

    public void ConnecTicket(S_Ticket ticket)
    {
        costumerTicket = ticket;
    }

    public void Despawn()
    {
        S_GameManager.TryDespawnCustomer(costumerTicket);
    }
    
}
