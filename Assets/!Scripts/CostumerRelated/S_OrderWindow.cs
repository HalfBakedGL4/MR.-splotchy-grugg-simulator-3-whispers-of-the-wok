using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;


[Serializable]
public class Order
{
    public DishType nameOfDish;
    public Sprite orderImage;
    public Sprite orderIngredients;
    public Sprite orderTools;
    public Sprite orderTutorial;
    
}
public class S_OrderWindow : MonoBehaviour
{
    [SerializeField] private S_Ticket ticketPrefab;
    [SerializeField] private List<Transform> ticketPlacements;
    [Tooltip("All possible dishes for costumers to order, with the descriptive images of items ordered")]
    [SerializeField] private List<Order> orderTypes = new List<Order>();

    
    private Dictionary<S_Ticket, Transform> ticketsDictionary = new Dictionary<S_Ticket, Transform>();

    // Costumer will request a Dish
    public S_Ticket MakeOrder(DishType dish, S_CostumerOrder costumer)
    {
        var thisOrder = new Order();
        var foundOrder = false;
        
        // Goes through list seeking order with the same dish
        foreach (var order in orderTypes.Where(order => order.nameOfDish == dish))
        {
            thisOrder = order;
            foundOrder = true;
            break;
        }

        if (foundOrder)
        {
            return AddTicket(thisOrder, costumer);
        }
        else
        {
            Debug.LogError("Dish does not exist or is not added to the Order Window");
            return null;
        }
    }

    // Ticket is added
    private S_Ticket AddTicket(Order order, S_CostumerOrder costumer)
    {
        // Get random transform from List to place item
        var pos = ticketPlacements[Random.Range(0, ticketPlacements.Count - 1)];
        // Instantiate and place ticket on position
        var ticket = Instantiate(ticketPrefab, pos.position, quaternion.identity);
        
        // Remove position ticket can appear
        ticketPlacements.Remove(pos);
        // add to Dictionary
        ticketsDictionary[ticket] = pos;
        
        // Rotate Ticket to fit
        ticket.transform.eulerAngles = pos.eulerAngles;
        ticket.transform.parent = pos.parent;
        
        // Initiate ticket giving it the corresponding visuals to complete it
        ticket.InitTicket(order, costumer);
        
        // Ticket is returned to the costumer so the costumer know which ticket they own
        return ticket;
    }
    
    public void RemoveTicket(S_Ticket ticket)
    {
        // Find through Dictionary
        var pos = ticketsDictionary[ticket];
        // Add position back to List
        ticketPlacements.Add(pos);
        // Remove used ticket
        ticketsDictionary.Remove(ticket);
        // Destroy ticket from scene
        Destroy(ticket.gameObject);

    }

    public void DeliverOrder(SelectEnterEventArgs args)
    {
        GiveOrderToCostumer(args.interactableObject.transform.GetComponent<S_DishStatus>());
        RemoveDish(args.interactableObject.transform.gameObject);
    }
    
    private void GiveOrderToCostumer(S_DishStatus dish)
    {
        var possibleTickets = new List<S_Ticket>();
        // Compare dish with every ticket
        foreach (var ticket in ticketsDictionary.Keys)
        {
            // Save any ticket that matches the Type of Dish
            if (ticket.GetOrder().nameOfDish == dish.GetTypeOfDish())
            {
                possibleTickets.Add(ticket);
            }
        }
        
        // If there are no correct dishes, the dish will still be given to a costumer
        // Add all the orders to the list
        if (possibleTickets.Count == 0)
        {
            foreach (var ticket in ticketsDictionary.Keys)
            {
                possibleTickets.Add(ticket);
            }
        }

        S_Ticket correctTicket = possibleTickets[0];
        // Check which ticket is the oldest one aka has the lowest ID
        foreach (var ticket in possibleTickets)
        {
            // If correctTicket is higher swap out with new ticket
            if (correctTicket.GetTicketNumber() > ticket.GetTicketNumber())
            {
                correctTicket = ticket;
            }
        }
        
        // Give the order to the costumer through the ticket reference
        correctTicket.GetCostumer().ReceiveDish(dish);
    }

    private void RemoveDish(GameObject dish)
    {
        Destroy(dish);
    }
}