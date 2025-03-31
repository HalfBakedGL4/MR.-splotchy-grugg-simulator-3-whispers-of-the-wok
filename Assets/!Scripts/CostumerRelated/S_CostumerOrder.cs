using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_CostumerOrder : MonoBehaviour
{
    S_OrderWindow orderWindow;
    
    [Header("What Dishes the costumer can order")]
    [SerializeField] private List<Dish> canOrder = new ();

    private Dish _dishOrder;
    private void Start()
    {
        // Find the window to place ticket
        orderWindow = FindAnyObjectByType<S_OrderWindow>();

        _dishOrder = OrderFood();
    }
    
    // The costumer would order food when they approach the window
    public Dish OrderFood()
    {
        _dishOrder = canOrder[Random.Range(0, canOrder.Count)];
        
        orderWindow.MakeOrder(_dishOrder);
        
        return _dishOrder;
    }
}
