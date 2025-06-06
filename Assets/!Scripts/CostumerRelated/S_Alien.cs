using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class S_Alien : NetworkBehaviour
{
    public NavMeshAgent agent;
    public float speed = 1;

    private List<S_Appliance> appliances;
    private int applianceIndex = -1;

    public override void Spawned()
    {
        appliances = FindObjectsByType<S_Appliance>(FindObjectsSortMode.None).ToList();
        Debug.Log("[Navmesh] Appliances found: " + appliances.Count);
        FollowAppliance();
        InvokeRepeating(nameof(FollowAppliance), 5, 10);
    }

    void FollowAppliance()
    {
        if (agent.isOnNavMesh)
        {

            applianceIndex = Random.Range(0, appliances.Count - 1);
            
            Vector3 targetPosition;

            targetPosition = appliances[applianceIndex].transform.position;
            //targetPosition = Camera.main.transform.position;

            agent.SetDestination(targetPosition);
            agent.speed = speed;
        }
        else
        {
            Debug.LogError("[Navmesh] Navmesh Agent not attached to Navmesh Surface.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GetComponent<S_CostumerOrder>().Despawn();
            //Runner.Despawn(GetComponent<NetworkObject>());
        }
    }
}
