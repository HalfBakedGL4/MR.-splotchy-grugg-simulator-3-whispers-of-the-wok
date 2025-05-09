using System;
using System.Collections.Generic;
using System.Net.Mime;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class S_Ticket : NetworkBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform ticketSpawnPoint;
    [SerializeField] private S_TicketDetailGiver ticketDetailGiver;
    [SerializeField] private TextMeshProUGUI ticketNumberText;
    
    private static int ticketNumber;
    
    bool isLocal => Object && Object.HasStateAuthority;
    
    private Order _currentOrder;
    private S_CostumerOrder _costumerOrder;
    public void InitTicket(Order order, S_CostumerOrder costumerOrder)
    {
        if (!isLocal) {return;}
        
        ticketDetailGiver = Runner.Spawn(prefab, ticketSpawnPoint.position, ticketSpawnPoint.rotation).GetComponent<S_TicketDetailGiver>();
        
        //Number the ticket
        ticketNumber++;
        
        ticketDetailGiver.InitTicket(order, ticketNumber);
        
        // Save the order and costumer who ordered so that they can be found via the ticket later
        _currentOrder = order;
        _costumerOrder = costumerOrder;
        ticketNumberText.text = $"#{ticketNumber}";
    }

    public void DestroyTicketDetails()
    {
        Runner.Despawn(ticketDetailGiver.GetComponent<NetworkObject>());
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

    
}
