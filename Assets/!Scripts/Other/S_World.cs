using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class S_World : MonoBehaviour
{
    public static List<S_Food> currentFood { get; private set; } = new List<S_Food>();
    public const int maxFood = 10;

    /// <summary>
    /// used to instantiate food
    /// </summary>
    public static S_Food InstantiateFood(S_Food food, Vector3 position, quaternion rotation)
    {
        if (currentFood.Count >= maxFood) return null;

        S_Food instantiatedFood = Instantiate(food, position, rotation);
        currentFood.Add(instantiatedFood);

        return instantiatedFood;
    }
    /// <summary>
    /// used to instantiate food
    /// </summary>
    public static S_Food InstantiateFood(S_Food food, Transform parent)
    {
        S_Food instantiatedFood = InstantiateFood(food, parent.position, parent.rotation);
        if (instantiatedFood == null) return null;

        instantiatedFood.transform.parent = parent;
        instantiatedFood.transform.localPosition = Vector3.zero;
        instantiatedFood.transform.localEulerAngles = Vector3.zero;

        return instantiatedFood;
    }

    /// <summary>
    /// used to destroy food
    /// </summary>
    public static void DestroyFood(Object food, float timer = 0)
    {
        S_Food toDestroy = food.GetComponent<S_Food>();
        if (toDestroy == null) return;

        currentFood.Remove(toDestroy);
        Destroy(toDestroy, timer);
    }
}
