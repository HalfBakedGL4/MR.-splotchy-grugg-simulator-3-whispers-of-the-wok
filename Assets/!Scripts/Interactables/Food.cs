using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;

    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    private void Start() 
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    public void TakeCuts(float damageAmount) 
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
    }
}
