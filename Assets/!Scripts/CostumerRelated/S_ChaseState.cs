using Oculus.Interaction.Surfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class S_ChaseState : S_State
{
    public NavMeshAgent agent;
    public S_AttackState attackState;
    public float speed = 1;
    public bool isInAttackRange;

    public override S_State RunCurrentState() 
    {
        if (isInAttackRange) 
        {
            return attackState;
        }
        else 
        {
            return this;
        }
    }
    private void Awake()
    {
        
    }
    IEnumerator Start()
    {
        yield return new WaitUntil(() => GetComponent<NavMeshSurface>() != null);
    }
    private void Update()
    {
        if (Camera.main == null) return;

        if (agent.isOnNavMesh)
        {
            Vector3 targetPosition = Camera.main.transform.position;

            agent.SetDestination(targetPosition);
            agent.speed = speed;
        }
        else
        {
            Debug.LogError("[Navmesh] Navmesh Agent not attached to Navmesh Surface.");
        }
    }
}
