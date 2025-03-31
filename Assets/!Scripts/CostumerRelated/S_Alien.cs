using Meta.XR.MRUtilityKit;
using Oculus.Interaction.Surfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class S_Alien : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitUntil(() => GetComponent<NavMeshSurface>() != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isOnNavMesh)
        {
            /*Vector3 targetPosition = Camera.main.transform.position;

            agent.SetDestination(targetPosition);
            agent.speed = speed;*/
        } else
        {
            Debug.LogError("[Navmesh] Navmesh Agent not attached to Navmesh Surface.");
        }
    }
}
