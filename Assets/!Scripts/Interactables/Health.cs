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

    public UnityEvent OnDamage;
    public UnityEvent OnAnimation;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    private void Start() 
    {
        health = maxHealth;
    }

    void Update() 
    {
        // Increment Counter
        /*do 
        {
            coolDown += Time.deltaTime;
        } while (coolDown < 2);
        
        

        if (coolDown >= 2 && coolDownEnabled == false)
            coolDown = 0;
            
        The start of pseudocode to solve the issue of knife collider not
        fully exiting tomato and damaging more times with actual movement*/
    }

    private void OnTriggerEnter(Collider col) 
    {
        Debug.Log("trigger");
        if (col.tag == "Knife") 
        {
            Debug.Log("knife trigger");
            OnDamage?.Invoke();
            OnAnimation?.Invoke();
        }
        if (col.tag == "Wall") 
        {
            OnDamage?.Invoke(); // ChangeState(state);
        }
    }

    public void CutInSlices(float damageAmount) // (float damageAmount, int stateIndex) 
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
        // Chopping animations, wall breaks etc.
    }
}
