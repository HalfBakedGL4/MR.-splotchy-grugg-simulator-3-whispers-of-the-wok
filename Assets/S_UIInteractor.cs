using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class S_UIInteractor : MonoBehaviour
{
    [SerializeField] float length;
    [Range(0, 1)]
    [SerializeField] float radius;
    [SerializeField] InputActionReference toInteract;

    LayerMask uiLayer = 32;

    Ray uiRay => new Ray(transform.position, transform.forward);
    RaycastHit uiHit;

    bool isHititng;

    public UnityEvent started;
    public UnityEvent preformed;
    public UnityEvent cancelled;

    void Update()
    {
        bool hit = Physics.SphereCast(uiRay, radius, out uiHit, length, uiLayer);
        if (hit && isHititng)
        {
            if(uiHit.collider.TryGetComponent(out S_UIElement element))
            {
                element.OnHover();
                if (element is S_UIButton && toInteract.action.WasPressedThisFrame())
                {
                    //(S_UIButton)element.on
                }
            }

            preformed?.Invoke();
        } else if(hit && !isHititng)
        {
            if (uiHit.collider.TryGetComponent(out S_UIElement element))
            {
                element.OnHoverEnter();
            }

            started?.Invoke();
        } else if(!hit && isHititng)
        {
            if (uiHit.collider.TryGetComponent(out S_UIElement element))
            {
                element.OnHoverExit();
            }

            cancelled?.Invoke();
        }

        isHititng = hit;
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
