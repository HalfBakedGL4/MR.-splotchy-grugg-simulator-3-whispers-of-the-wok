using System;
using UnityEngine;

[Flags]
public enum DishStatus
{
    UnCooked,
    UnderCooked,
    Cooked,
    OverCooked,
    Burnt,
}

public class S_DishStatus : MonoBehaviour
{
    /// <summary>
    /// Status to see how cooked the dish is
    /// Used to save stuff on the dish that could be used to determine score. E.g. how cooked, has grugg
    /// </summary>
    private DishStatus currentDishStatus = DishStatus.UnCooked;

    private bool isGrugged = false;
    
    [SerializeField] private DishType typeOfDish;

    public void ChangeStatus(DishStatus newStatus)
    {
        currentDishStatus = newStatus;
        //TODO: Add visual based on cooked time
        print(name + " change status to " + currentDishStatus);
    }

    public void ApplyGrugg()
    {
        isGrugged = true;
        // TODO: Add visual that grugg is added
        print("Grugg Applied to " + name);
    }

    public (DishType typeOfDish, DishStatus dishStatus, bool grugged) GetDishStatus()
    {
        return (typeOfDish, currentDishStatus, isGrugged);
    }

    public DishType GetTypeOfDish()
    {
        return typeOfDish;
    }
}