using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class S_UIButton : S_UIElement
{
    Color spriteColor;
    [SerializeField] Color hoverColor = Color.white * 0.9f;
    [SerializeField] Color pressedColor = Color.white * 0.7f;

    [HideInInspector]
    public bool isPressed;

    [Space]
    public UnityEvent PressedEnter;
    public UnityEvent Pressed;
    public UnityEvent PressedExit;

    private void Start()
    {
        spriteColor = sprite.color;
    }
    private void Update()
    {
        if(isHovering)
        {
            if(isPressed)
            {
                sprite.color = pressedColor;
            } 
            else
            {
                sprite.color = hoverColor;
            }
        } else
        {
            sprite.color = spriteColor;
        }
    }
    public virtual void OnPressedEnter(S_UIInteractor interactor)
    {
        if (isPressed) return;

        interactor.hapticPlayer.SendHapticImpulse(interactor.hapicAmplitude, interactor.hapicDuration);
        isPressed = true;
        PressedEnter?.Invoke();

        Debug.Log("[UIinteractor] PressedEnter");
    }
    public virtual void OnPressed(S_UIInteractor interactor)
    {
        if (!isPressed) return;

        Pressed?.Invoke();

        Debug.Log("[UIinteractor] Pressed");
    }
    public virtual void OnPressedExit(S_UIInteractor interactor)
    {
        if (!isPressed) return;

        isPressed = false;
        PressedExit?.Invoke();

        Debug.Log("[UIinteractor] PressedExit");
    }
}
