using UnityEngine;
using UnityEngine.Events;

public class S_UIElement : MonoBehaviour
{
    private void OnValidate()
    {
        gameObject.layer = 5;
    }
    public virtual void OnHoverEnter()
    {
    }
    public virtual void OnHover()
    {
    }
    public virtual void OnHoverExit()
    {
    }
}
