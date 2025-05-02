using UnityEngine;
using UnityEngine.Events;

public class S_UIButton : S_UIElement
{
    [SerializeField] SpriteRenderer sprite;

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
        sprite.color = hoverColor;
        base.OnHoverEnter();
    }
    public override void OnHoverExit()
    {
        sprite.color = spriteColor;
        base.OnHoverExit();
    }
    public virtual void OnPressedEnter()
    {

    }
    public virtual void OnPressed()
    {

    }
    public virtual void OnPressedExit()
    {

    }

    private void Reset()
    {
        if(!TryGetComponent(out sprite))
        {
            sprite = gameObject.AddComponent<SpriteRenderer>();
        }
    }
}
