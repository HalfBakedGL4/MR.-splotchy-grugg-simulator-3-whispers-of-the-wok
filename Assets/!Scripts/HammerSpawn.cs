using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HammerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject hammerPrefab;
    [SerializeField] private float startSpeed = 2;
    [SerializeField] private InputActionProperty inputAction;
    private bool hammerSpawned;
    
    
    void Update()
    {
        if (inputAction.action.WasPressedThisFrame() && hammerSpawned == false)
        {
            GameObject spawnedCub = Instantiate(hammerPrefab, transform.position, transform.rotation);
            Rigidbody cubeRigidbody = spawnedCub.GetComponent<Rigidbody>();
            cubeRigidbody.linearVelocity = transform.forward * startSpeed;
            hammerSpawned = true;
        }
    }
}
