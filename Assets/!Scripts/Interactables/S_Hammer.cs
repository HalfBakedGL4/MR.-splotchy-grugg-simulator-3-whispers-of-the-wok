using Fusion;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_Hammer : NetworkBehaviour
{
    [Header("Damage to objects")]
    [SerializeField] float wallMultiplier;
    [SerializeField] float enemyMultiplier;
    [Networked, OnChangedRender(nameof(NetworkUpdateVisuals))]
    public float charge { get; set; }
    private Rigidbody rb;
    NetworkRunner runner;
    public bool IsLocalNetworkRig => Object.HasInputAuthority;

    //For testing
    [SerializeField] Image fillImage;

    public override void Spawned()
    {
        rb = GetComponent<Rigidbody>();
        if (!IsLocalNetworkRig) enabled = false;

        runner = FindFirstObjectByType<NetworkRunner>();
    }

    private void Update()
    {
        if (!Object.HasStateAuthority) return;
        if(charge > 1) return;
        charge += 4*Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent != null)
        {
            if(other.TryGetComponent(out S_HoleManager holemanager))
            {
                holemanager.RPCHammerHit(charge * wallMultiplier);
            }
            if (other.gameObject.CompareTag("Enemy"))
            {
                DealDamage(Mathf.Clamp(charge, 0, 1), other.gameObject);
            }
        }
    }

    void DealDamage(float charge, GameObject hit)
    {
        if (hit.TryGetComponent<Health>(out Health health))
        {
            health.Damage(charge * enemyMultiplier);
        }
    }

    void NetworkUpdateVisuals()
    {
        fillImage.fillAmount = charge;
        // Update visuals for all players here
    }
}
