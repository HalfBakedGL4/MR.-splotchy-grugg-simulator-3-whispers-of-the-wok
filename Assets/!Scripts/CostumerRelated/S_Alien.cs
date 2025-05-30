using Fusion;
using Meta.XR.MRUtilityKit;
using NUnit.Framework;
using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class S_Alien : NetworkBehaviour
{
    public NavMeshAgent agent;
    public float speed = 1;

    private List<S_Appliance> appliances;

    public override void Spawned()
    {
        // Start coroutine manually since there's no automatic Start() in Fusion
        StartCoroutine(InitializeAfterSpawn());
    }

    private IEnumerator InitializeAfterSpawn()
    {
        appliances = FindObjectsByType<S_Appliance>(FindObjectsSortMode.None).ToList();
        if (appliances == null || appliances.Count == 0)
        {
            Debug.LogError("[Navmesh] No appliances found in the scene.");
        }
        yield return new WaitUntil(() => (GetComponent<NavMeshSurface>() != null && appliances != null));
    }

    // Update is called once per frame
    void Update()
    {
        if (appliances != null)
        {
            if (appliances.Count == 0)
            {
                Debug.LogError("[Navmesh] No appliances found in the scene.");
                return;
            }
            if (agent.isOnNavMesh)
            {
                followAppliance();
            }
            else
            {
                Debug.LogError("[Navmesh] Navmesh Agent not attached to Navmesh Surface.");
            }
        }
        else
        {
            Debug.LogError("[Navmesh] Appliances list is null or empty.");
        }
    }

    void followAppliance()
    {
        if (Camera.main == null) return;

        if (agent.isOnNavMesh)
        {
            Vector3 targetPosition;
            int x = Random.Range(0, appliances.Count - 1);

            if (x < 0 || x >= appliances.Count)
            {
                Debug.LogError("[Navmesh] Random index out of bounds: " + x);
                return;
            }

            targetPosition = appliances[x].transform.position;

            agent.SetDestination(targetPosition);
            agent.speed = speed;
        }
        else
        {
            Debug.LogError("[Navmesh] Navmesh Agent not attached to Navmesh Surface.");
        }
    }
}
