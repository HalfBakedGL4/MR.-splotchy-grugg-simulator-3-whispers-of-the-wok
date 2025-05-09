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
        Invoke("MadeOrder", 5);
    }

    void MadeOrder()
    {
        renderer.enabled = false;
        Invoke("Attack", 5);
    }

    void Attack()
    {
        // Enable renderer
        renderer.enabled = true;
        // Set position to behind wall
        (ARPlane a ,Vector3 pointOnWall) = FindFirstObjectByType<S_FindPointsOnWalls>().GetRandomWallAndPoint();
        pointOnWall.y = transform.localScale.y/2;
        transform.position = pointOnWall;

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
