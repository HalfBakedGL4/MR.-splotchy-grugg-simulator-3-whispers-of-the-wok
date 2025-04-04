using Fusion;
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
        charge += 1 * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.parent != null)
        {
            DealDamage(Mathf.Clamp(charge, 0, 1));
        }
    }

    void DealDamage(float charge)
    {
        
    }

    void NetworkUpdateVisuals()
    {
        fillImage.fillAmount = charge;
        // Update visuals for all players here
    }
}
