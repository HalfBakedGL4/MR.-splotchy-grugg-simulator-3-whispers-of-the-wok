using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_CostumerOrder : MonoBehaviour
{
    S_OrderWindow orderWindow;
    
    [SerializeField] private List<Dish> canOrder = new List<Dish>();

    private Dish _dishOrder;
    private void Start()
    {
        // Find the window to place ticket
        orderWindow = FindAnyObjectByType<S_OrderWindow>();

        OrderFood();
    }
    
    // The costumer would order food when they approach the window
    public void OrderFood()
    {
        _dishOrder = canOrder[Random.Range(0, canOrder.Count)];
        
        orderWindow.MakeOrder(_dishOrder);
    }
}
