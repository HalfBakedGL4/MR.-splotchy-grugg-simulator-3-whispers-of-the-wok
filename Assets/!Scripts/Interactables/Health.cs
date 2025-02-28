using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    //[SerializeField] int state; List

    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;

    public UnityEvent OnDamage;

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

    private void OnTriggerEnter(Collider col) 
    {
        if (tag == "Knife") 
        {
            OnDamage?.Invoke();
        }
        if (tag == "Wall") 
        {
            OnDamage?.Invoke();
        }
    }

    public void CutInSlices(float damageAmount) 
    {
        if (health == 0)
            return;

        health -= damageAmount;

        if (healthBar != null)
            healthBar.UpdateHealthBar(health, maxHealth);

        //state++;
        //ChangeState(state);
    }

    private void ChangeState(int stateIndex) 
    {

    }
}
