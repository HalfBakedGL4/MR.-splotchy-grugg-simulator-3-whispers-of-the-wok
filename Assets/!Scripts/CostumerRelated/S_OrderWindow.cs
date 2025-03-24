using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[Serializable]
public class Order
{
    public Dish nameOfDish;
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
    public void MakeOrder(Dish dish)
    {
        var thisOrder = new Order();
        
        // Goes through list seeking order with the same dish
        foreach (var order in orderTypes.Where(order => order.nameOfDish == dish))
        {
            thisOrder = order;
            break;
        }
        
        AddTicket(thisOrder);
    }

    // Ticket is added
    private void AddTicket(Order order)
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
        
        ticket.InitTicket(order);
    }

    public void RemoveTicket(S_Ticket sTicket)
    {
        // Find through Dictionary
        var pos = ticketsDictionary[sTicket];
        // Add position back to List
        ticketPlacements.Add(pos);
        // Remove used ticket
        ticketsDictionary.Remove(sTicket);

    }
}
