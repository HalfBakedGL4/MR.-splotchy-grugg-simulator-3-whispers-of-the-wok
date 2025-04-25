using Fusion;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class S_World : NetworkBehaviour
{
    static S_World instance;
    public static List<S_Food> currentFood { get; private set; } = new List<S_Food>();
    public const int maxFood = 10;

    private void Start()
    {
        instance = this;
    }

    /// <summary>
    /// used to spawn food
    /// </summary>
    public static S_Food SpawnFood(S_Food food, Vector3 position, Quaternion rotation)
    {
        if (currentFood.Count >= maxFood) return null;

        if (food == null)
        {
            Debug.LogError("Oh No, No food");
            return null;
        }

        if (instance == null)
        {
            Debug.LogError("Oh No, No instance");
            return null;
        }

        if (instance.Runner == null)
        {
            Debug.LogError("Oh No, No runner");
            return null;
        }

        S_Food instantiatedFood = instance.Runner.Spawn(food.GetComponent<NetworkObject>(), position, rotation).GetComponent<S_Food>();

        if (instantiatedFood == null) return null;
        currentFood.Add(instantiatedFood);

        return instantiatedFood;
    }

    /// <summary>
    /// used to destroy food
    /// </summary>
    public static void DespawnFood(S_Food food)
    {
        S_Food toDestroy = food.GetComponent<S_Food>();
        if (toDestroy == null) return;

        currentFood.Remove(toDestroy);
        instance.Runner.Despawn(toDestroy.GetComponent<NetworkObject>());
    }
}
