using System;
using UnityEngine;

public class S_GruggJuice : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out S_DishStatus dishStatus))
        {
            dishStatus.ApplyGrugg();
        }
    }
}
