using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
public class Health : MonoBehaviour
{
    /*[SerializeField] float coolDown; // in seconds
    bool coolDownEnabled = false;*/

    //[SerializeField] int state; List

    [SerializeField] float health, maxHealth = 3;


    [SerializeField] FloatingHealthBar healthBar;

    // Adding UnityEvents for Damage and Healing
    public UnityEvent OnDamage;
    public UnityEvent OnHealing;

    public UnityEvent OnAnimation;
    public UnityEvent OnChop;

    //int currentChild = 2;
    GameObject child;

    private void Awake() 
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    private void Start() 
    {
        health = maxHealth;
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

    public void Damage(float damageAmount) 
    {
        OnDamage?.Invoke();

        UpdateHealth(-damageAmount);
        CheckDeath();
    }

    public void ChopObject(Collider col)
    {
        OnChop?.Invoke();

        Debug.Log(transform.childCount);

        for (int i = 0; i < transform.childCount; i++) 
        {
            child = transform.GetChild(i).gameObject;
            if (child == col.gameObject)
            {
                break;
            }
        }

        //child = transform.GetChild(transform.childCount-1).gameObject;
        child.AddComponent<Rigidbody>();
        child.AddComponent<XRGrabInteractable>();
        child.transform.parent = null;


        /*if (currentChild == 0)
        {
            Destroy(gameObject);
        }
        currentChild--;*/

        if (transform.childCount == 0) Destroy(gameObject);
    }

    public void Healing(float healingAmount) 
    {
        OnHealing?.Invoke();

        UpdateHealth(healingAmount);
    }

    private void ChangeState(int stateIndex) 
    {
        // Chopping animations, wall breaks etc.
    }
}
