using UnityEngine;
using UnityEngine.Events;

public class S_UIElement : MonoBehaviour
{
    public UnityEvent HoverEnter;
    public UnityEvent Hover;
    public UnityEvent HoverExit;
    public virtual void OnHoverEnter()
    {
        HoverEnter?.Invoke();
    }
    public virtual void OnHover()
    {
        Hover?.Invoke();
    }
    public virtual void OnHoverExit()
    {
        HoverExit?.Invoke();
    }
}
