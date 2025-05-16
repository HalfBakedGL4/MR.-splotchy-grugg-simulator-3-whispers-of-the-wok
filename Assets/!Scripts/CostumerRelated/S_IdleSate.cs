using Fusion;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class S_IdleSate : S_State
{
    public S_ChaseState chaseState;
    public bool canSeeThePlayer = false;
    Renderer renderer;
    [SerializeField] int foodWaitingTime;

    private void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
        Debug.Log("Customer making order");
        Invoke("MadeOrder", 5);
    }


    void MadeOrder()
    {
        //renderer.enabled = false;
        Debug.Log("Customer made order");
        Invoke("Attack", 5);
    }

    void Attack()
    {
        Debug.Log("Customer started chase state");
        // Enable renderer
        //renderer.enabled = true;

        // Set position to behind wall
        (ARPlane a ,Vector3 pointOnWall) = FindFirstObjectByType<S_FindPointsOnWalls>().GetRandomWallAndPoint();
        Debug.Log("Spawning on wall: "+a.gameObject.name);

        pointOnWall.y = transform.localScale.y/2;
        transform.position = pointOnWall;

        // Spawn hole
        S_HoleSpawner holeSpawner = FindFirstObjectByType<S_HoleSpawner>();
        holeSpawner.SpawnHole(pointOnWall, a.gameObject.transform.localRotation);

        // Swap state to chase
        canSeeThePlayer = true;
    }


    public override S_State RunCurrentState() 
    {
        if (canSeeThePlayer) 
        {
            return chaseState;
        }
        else 
        {
            return this;
        }
    }
}
