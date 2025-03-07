using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    /*[SerializeField] float coolDown; // in seconds
    bool coolDownEnabled = false;*/

    //[SerializeField] int state; List

    [SerializeField] float health, maxHealth = 3f;

    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;

    // Adding UnityEvents for Damage and Healing
    public UnityEvent OnDamage;
    public UnityEvent OnHealing;

    public UnityEvent OnAnimation;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    private void Start() 
    {
        health = maxHealth;

        OnDamage?.Invoke();
        OnHealing?.Invoke();
    }


    /*
    void Update() 
    {
        // Increment Counter
        do 
        {
            coolDown += Time.deltaTime;
        } while (coolDown < 2);
        
        

        if (coolDown >= 2 && coolDownEnabled == false)
            coolDown = 0;
            
        The start of pseudocode to solve the issue of knife collider not
        fully exiting tomato and damaging more times with actual movement
    }

    private void OnTriggerEnter(Collider col) 
    {
        Debug.Log("trigger");
        if (col.tag == "Knife") 
        {
            Debug.Log("knife trigger");
            OnDamage?.Invoke();
            //OnAnimation?.Invoke();
        }
        if (col.tag == "Wall") 
        {
            OnDamage?.Invoke(); // ChangeState(state);
        }
    }*/


    // Function for updating health with damage dealt or health healed
    private void UpdateHealth(float healthAmount) 
    {
        health += healthAmount;

        if (healthBar != null)
            healthBar.UpdateHealthBar(health, maxHealth);
    }

    private void CheckDeath() 
    {
        if (health <= 0) 
        {
            Destroy(gameObject);
        }
    }

    public void CutInSlicesFood(float damageAmount) // (float damageAmount, int stateIndex) 
    {
        UpdateHealth(-damageAmount);
        CheckDeath();

        //state++;
        //ChangeState(state);
    }

    public void BreakJointsWall(float damageAmount) 
    {
        UpdateHealth(-damageAmount);
        CheckDeath();
    }

    public void DamageCustomer(float damageAmount) 
    {
        UpdateHealth(-damageAmount);
        CheckDeath();
    }

    public void HealingCustomer(float healingAmount) 
    {
        UpdateHealth(healingAmount);
    }

    private void ChangeState(int stateIndex) 
    {
        // Chopping animations, wall breaks etc.
    }
}
