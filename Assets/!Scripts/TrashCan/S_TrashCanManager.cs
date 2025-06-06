using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_TrashCanManager : NetworkBehaviour, IToggle
{
    [SerializeField] private Transform suckPoint;
    [SerializeField] private float suckForce = 10.0f;
    [SerializeField] private GameObject platePrefab;
    [SerializeField] private Animator anim;

    [Header("Child Scripts")]
    [SerializeField] private S_DestroyTrash destroyTrash;
    [SerializeField] private S_MoveTrash moveTrash;
    [SerializeField] private S_PlateDispenser[] plateDispenserScripts;
    
    [Header("Particle Effects")]
    [SerializeField] private List<ParticleSystem> trashParticles;
    
    [Networked] private bool isTurnedOn { get; set; }

    bool isLocal => Object && Object.HasStateAuthority;


    public override void Spawned()
    {
        base.Spawned();
        
        ConnectToApplicationManager();
        // Add listener to event by the GameManager That calls CleanUpFloor
        if (!HasStateAuthority) return;
        S_GameManager.OnFoodListFull += CleanUpFloor;
        foreach (var plateDispenser in plateDispenserScripts)
        {
            plateDispenser.OnPlateRemoved += AddNewPlate;
        }
    }

    public void ConnectToApplicationManager()
    {
        if (S_ApplicationManager.Instance != null)
        {
            S_ApplicationManager.Instance.RegisterToggle(this);
        }
    }

    // Whenever Game Manager Food list is full the trash will try to clean up the scene
    // Moves any food on the floor to the trash can
    private void CleanUpFloor()
    {
        if (!isTurnedOn || !isLocal) {return;}
        
        var foodInScene = FindObjectsByType<S_Food>(FindObjectsSortMode.None);
        var dishInScene = FindObjectsByType<S_DishStatus>(FindObjectsSortMode.None);

        List<GameObject> foodOnFloor = new List<GameObject>();

        foreach (var food in foodInScene)
        {
            if (food.transform.position.y < .3f)
            {
                foodOnFloor.Add(food.gameObject);
            }
        }

        foreach (var dish in dishInScene)
        {
            if (dish.transform.position.y < .3f)
            {
                foodOnFloor.Add(dish.gameObject);
            }
        }

        // Make food on floor move towards suckPoint to be deleted
        RPC_StartSucking();
        StartCoroutine(SuckFoodCoroutine(foodOnFloor));

    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StartSucking()
    {
        foreach (var particle in trashParticles)
        {
            particle.Play();
        }
    }
    private IEnumerator SuckFoodCoroutine(List<GameObject> foodOnFloor)
    {
        List<Rigidbody> foodRBs = foodOnFloor.Select(food => food.GetComponent<Rigidbody>()).ToList();

        while (foodRBs.Count > 0)
        {
            foreach (var food in foodRBs.ToList().Where(food => food))
            {
                food.AddForce((suckPoint.position - food.transform.position).normalized * suckForce);

                if (Vector3.Distance(food.transform.position, suckPoint.position) < 0.1f)
                {
                    foodRBs.Remove(food);
                }
            }

            yield return null; // wait one frame so physics can update
        }
    }


    private void AddNewPlate(Transform spawnPoint)
    {
        Runner.Spawn(platePrefab, spawnPoint.position, spawnPoint.rotation);
    }
    
    public virtual void OnDisable()
    {
        if (!HasStateAuthority) { return; }
        S_GameManager.OnFoodListFull -= CleanUpFloor;
        foreach (var plateDispenser in plateDispenserScripts)
        {
            plateDispenser.OnPlateRemoved -= AddNewPlate;
        }
    }

    public void SetApplicationActive(bool toggle)
    {
        isTurnedOn = toggle;
        
        
        print(name + " is turned on: " + toggle);

        RPC_ToggleMovement(toggle);

    }

    private XRGrabInteractable _grabInteractable;
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_ToggleMovement(bool toggle)
    {
        destroyTrash.enabled = toggle;
        moveTrash.enabled = toggle;
        anim.enabled = toggle;   
        
        if (_grabInteractable == null)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }
        
        // Is opposite of toggle because it needs to be on when everything is off
        _grabInteractable.enabled = !toggle;
    }
}
