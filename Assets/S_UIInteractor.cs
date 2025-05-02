using Input;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class S_UIInteractor : MonoBehaviour
{
    [SerializeField] float length;
    [Range(0, 1)]
    [SerializeField] float radius;

    LayerMask uiLayer = 32;

    Ray uiRay => new Ray(transform.position, transform.forward);
    RaycastHit uiHit;

    S_UIElement hitting;

    void Update()
    {
        bool hit = Physics.SphereCast(uiRay, radius, out uiHit, length, uiLayer);
        Debug.Log(S_InputReader.instance);

        if(hit && hitting == null)
        {
            if (uiHit.collider.TryGetComponent(out S_UIElement element))
            {
                element.OnHoverEnter();
                hitting = element;

                S_InputReader.instance.RightA.AddListener(PressButton);
            }
        }
        
        if(!hit && hitting != null)
        {
            hitting.OnHoverExit();
            hitting = null;

            S_InputReader.instance.RightA.RemoveListener(PressButton);
        }

        if (hit)
        {
            if (uiHit.collider.TryGetComponent(out S_UIElement element))
            {
                if(element != hitting)
                {
                    hitting.OnHoverExit();
                }

                hitting = element;

                element.OnHover();
            }
        }
    }

    private void PressButton(InputInfo info)
    {
        Debug.Log("[UIinteractor] pressing");
        if (!(hitting is S_UIButton)) return;

        if (!info.context.started)
        {
            ((S_UIButton)hitting).OnPressedEnter();
        }

        if (!info.context.performed)
        {
            ((S_UIButton)hitting).OnPressed();
        }

        if (!info.context.canceled)
        {
            ((S_UIButton)hitting).OnPressedExit();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + transform.up * radius, transform.position + transform.up * radius + transform.forward * length);
        Gizmos.DrawLine(transform.position + transform.right * radius, transform.position + transform.right * radius + transform.forward * length);
        Gizmos.DrawLine(transform.position + -transform.up * radius, transform.position + -transform.up * radius + transform.forward * length);
        Gizmos.DrawLine(transform.position + -transform.right * radius, transform.position + -transform.right * radius + transform.forward * length);

        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position + transform.forward * length, radius);
    }
}
