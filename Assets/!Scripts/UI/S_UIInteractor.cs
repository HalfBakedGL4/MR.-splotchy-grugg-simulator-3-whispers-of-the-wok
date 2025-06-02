using Input;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

[RequireComponent(typeof(HapticImpulsePlayer))]
public class S_UIInteractor : MonoBehaviour
{
    [SerializeField, Min(0f)] float length = 3;
    [Range(0, 1)]
    [SerializeField] float radius = .1f;

    [Space]

    [Range(0, 1)] public float hapicAmplitude = .3f;
    [Range(0, 1)] public float hapicDuration = .1f;

    [Space]

    [SerializeField] GameObject indicator;


    LayerMask uiLayer = 32;

    Ray uiRay => new Ray(transform.position, transform.forward);
    RaycastHit uiHit;

    S_UIElement hitting;

    [HideInInspector] public HapticImpulsePlayer hapticPlayer;

    private void Start()
    {
        hapticPlayer = GetComponent<HapticImpulsePlayer>();
    }

    void Update()
    {
        bool hit = Physics.SphereCast(uiRay, radius, out uiHit, length, uiLayer);
        Debug.Log(S_InputReader.instance);

        if (!hit && hitting != null)
        {
            hitting.OnHoverExit(this);
            hitting = null;

            S_InputReader.instance.RightA.RemoveListener(PressButton);
        }

        if (hit)
        {
            uiHit.collider.TryGetComponent(out S_UIElement element);

            if (hitting == null)
            {
                element.OnHoverEnter(this);
                hitting = element;

                S_InputReader.instance.RightA.AddListener(PressButton);
            }

            if (element != hitting)
            {
                hitting.OnHoverExit(this);
            }

            hitting = element;

            indicator.SetActive(true);
            indicator.transform.position = uiHit.point;

            element.OnHover(this);
        } 
        else
        {
            indicator.SetActive(false);
        }
    }

    public void PressButton(InputInfo info)
    {
        if (!(hitting is S_UIButton)) return;

        if (info.context.started)
        {
            Debug.Log("[UIinteractor] pressing started");
            ((S_UIButton)hitting).OnPressedEnter(this);
        }

        if (info.context.performed)
        {
            Debug.Log("[UIinteractor] pressing");
            ((S_UIButton)hitting).OnPressed(this);
        }

        if (info.context.canceled)
        {
            Debug.Log("[UIinteractor] pressing canceled");
            ((S_UIButton)hitting).OnPressedExit(this);
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
