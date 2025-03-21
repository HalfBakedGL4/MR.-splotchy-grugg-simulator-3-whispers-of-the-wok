using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[System.Serializable]
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
    [SerializeField] private Ticket ticketPrefab;
    [SerializeField] private List<Transform> ticketPlacements;
    [Tooltip("All possible dishes for costumers to order, with the images of items ordered")]
    [SerializeField] private List<Order> orderTypes = new List<Order>();

    
    private Dictionary<Ticket, Transform> ticketsDictionary = new Dictionary<Ticket, Transform>();
    private void Start()
    {
        AddTicket(orderTypes[0]);
    }

    public void AddTicket(Order order)
    {
        var ticket = Instantiate(ticketPrefab);
        // Get random transform from List
        var pos = ticketPlacements[Random.Range(0, ticketPlacements.Count - 1)];
        // Remove position ticket can appear
        ticketPlacements.Remove(pos);
        // add to Dictionary
        ticketsDictionary[ticket] = pos;
        
        ticket.transform.position = pos.position;
        
        ticket.InitTicket(order);
    }

    public void RemoveTicket(Ticket ticket)
    {
        // Find through Dictionary
        var pos = ticketsDictionary[ticket];
        // Add position back to List
        ticketPlacements.Add(pos);
        // Remove used ticket
        ticketsDictionary.Remove(ticket);

    }
}
