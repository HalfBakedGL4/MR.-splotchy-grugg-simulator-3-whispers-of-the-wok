using Unity.Mathematics;
using UnityEngine;

public class S_World : MonoBehaviour
{
    public static int currentFoodAmount { get; private set; }
    public const int maxFood = 10;

    /// <summary>
    /// used to instantiate food
    /// </summary>
    public static object InstantiateFood(Object food, Vector3 position, quaternion rotation)
    {
        if (currentFoodAmount >= maxFood) return null;

        currentFoodAmount++;
        return Instantiate(food, position, rotation);
    }
    /// <summary>
    /// used to instantiate food
    /// </summary>
    public static object InstantiateFood(Object food, Transform parent)
    {
        if (currentFoodAmount >= maxFood) return null;

        currentFoodAmount++;
        return Instantiate(food, parent);
    }

    /// <summary>
    /// used to destroy food
    /// </summary>
    public static void DestroyFood(Object food, float timer = 0)
    {
        Destroy(food, timer);
        currentFoodAmount--;
    }
}
