using UnityEngine;
using UnityEngine.Events;

public class S_UIButton : S_UIElement
{
    Color spriteColor;
    [SerializeField] Color hoverColor = Color.white * 0.9f;
    [SerializeField] Color pressedColor = Color.white * 0.7f;

    [Space]
    public UnityEvent PressedEnter;
    public UnityEvent Pressed;
    public UnityEvent PressedExit;

    private void Start()
    {
        spriteColor = sprite.color;
    }

    public override void OnHoverEnter()
    {
        Debug.Log("[UIinteractor] HoverEnter");
        sprite.color = hoverColor;
        base.OnHoverEnter();
    }
    public override void OnHoverExit()
    {
        Debug.Log("[UIinteractor] HoverExit");
        sprite.color = spriteColor;
        base.OnHoverExit();
    }
    public virtual void OnPressedEnter()
    {
        Debug.Log("[UIinteractor] PressedEnter");
    }
    public virtual void OnPressed()
    {
        Debug.Log("[UIinteractor] Pressed");
        sprite.color = pressedColor;
    }
    public virtual void OnPressedExit()
    {
        Debug.Log("[UIinteractor] PressedExit");
    }
}
