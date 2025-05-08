using NaughtyAttributes;
using Oculus.Platform.Models;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider))]
public class S_UIElement : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected Collider col;

    [HideInInspector]
    public bool isHovering;


    public virtual void OnHoverEnter(S_UIInteractor interactor)
    {
        interactor.hapticPlayer.SendHapticImpulse(interactor.hapicAmplitude, interactor.hapicDuration);
        Debug.Log("[UIinteractor] HoverEnter");
        isHovering = true;
    }
    public virtual void OnHover(S_UIInteractor interactor)
    {
    }
    public virtual void OnHoverExit(S_UIInteractor interactor)
    {
        Debug.Log("[UIinteractor] HoverExit");
        isHovering = false;
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
