using UnityEngine;

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
    private DishStatus currentDishStatus = DishStatus.UnCooked;

    private bool isGrugged = false;

    public void ChangStatus(DishStatus newStatus)
    {
        currentDishStatus = newStatus;
        print(name + " change status to " + currentDishStatus);
    }

    public void ApplyGrugg()
    {
        isGrugged = true;
        print("Grugg Applied to " + name);
    }
    
}