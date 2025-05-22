using System;
using Fusion;
using UnityEngine;

public class S_DestroyTrash : NetworkBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out S_Food food))
        {
            S_GameManager.TryDespawnFood(food);
        }
        else if(other.gameObject.TryGetComponent(out S_DishStatus dish))
        {
            Runner.Despawn(dish.GetComponent<NetworkObject>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out S_Food food))
        {
            S_GameManager.TryDespawnFood(food);
        }
        else if(other.TryGetComponent(out NetworkObject trash))
        {
            Runner.Despawn(trash);
        }
    }
}
