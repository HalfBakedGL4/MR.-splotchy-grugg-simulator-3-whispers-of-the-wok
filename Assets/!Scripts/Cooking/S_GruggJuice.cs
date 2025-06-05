using System;
using Fusion;
using UnityEngine;

public class S_GruggJuice : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out S_DishStatus dishStatus))
        {
            dishStatus.ApplyGrugg();
            
            // Make material Yellow for visual
            GruggDish(other.gameObject);
        }
    }

    private void GruggDish(GameObject dish)
    {
        var rends = dish.GetComponentsInChildren<Renderer>();

        foreach (var rend in rends)
        {
            rend.material.color = Color.yellow;
        }
    }
}
