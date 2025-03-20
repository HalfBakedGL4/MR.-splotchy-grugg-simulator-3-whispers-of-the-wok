using DG.Tweening;
using UnityEngine;

public class WallExplosion : MonoBehaviour
{
    
    [SerializeField] private float timeUntilFade;   // Time before object starts to fade.
    [SerializeField] private float fadeDuration; // time before object is removed.

    private MeshRenderer[] fadeMaterial;
    private Rigidbody[] rb;
    
    private void Start()
    {
        fadeMaterial = GetComponentsInChildren<MeshRenderer>();
        rb = GetComponentsInChildren<Rigidbody>();

        // Add explosion force to all wall pieces.
        foreach (var rb in rb)
        {
            rb.AddExplosionForce(200, transform.parent.position, 2);
        }

        // Un-parents itself from the hole, so it wont despawn when hole gets fixed.
        transform.parent = null;

        // Fading out wall pieces after amount of time.
        Invoke("FadeOut", timeUntilFade);
        Invoke("RemovePieces", fadeDuration+timeUntilFade);
    }

    void FadeOut()
    {
        // Fades out all child with a material, Using DOtween asset.
        foreach (var material in fadeMaterial)
        {
            material.material.DOFade(0, fadeDuration);
        }
    }

    void RemovePieces()
    {
        Destroy(gameObject);
    }
}
