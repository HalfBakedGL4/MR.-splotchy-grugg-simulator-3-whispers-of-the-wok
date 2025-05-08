using UnityEngine;

public class S_DestroyTrash : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out S_Food food))
        {
            S_GameManager.DespawnFood(food);
        }
        else if(other.gameObject.TryGetComponent(out S_DishStatus dish))
        {
            Destroy(dish);
        }
    }
}
