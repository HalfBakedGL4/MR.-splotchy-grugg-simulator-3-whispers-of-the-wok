using UnityEngine;
using UnityEngine.Events;

public class S_UIButton : S_UIElement
{
    [Space]
    public UnityEvent PressedEnter;
    public UnityEvent Pressed;
    public UnityEvent PressedExit;
    public override void OnHover()
    {
        base.OnHover();
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
}
