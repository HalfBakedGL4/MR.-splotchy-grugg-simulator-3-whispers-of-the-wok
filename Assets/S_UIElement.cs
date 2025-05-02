using Oculus.Platform.Models;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider))]
public class S_UIElement : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected Collider col;


    public virtual void OnHoverEnter()
    {
    }
    public virtual void OnHover()
    {
    }
    public virtual void OnHoverExit()
    {


    }

    private void Reset()
    {
        gameObject.layer = 5;

        TryGetComponent(out sprite);
        if (!TryGetComponent(out col))
        {
            col = gameObject.AddComponent<BoxCollider>();
        }

    }
    private void OnValidate()
    {
        gameObject.layer = 5;

        TryGetComponent(out sprite);
        if(!TryGetComponent(out col))
        {
            col = gameObject.AddComponent<BoxCollider>();
        }

    }
}
