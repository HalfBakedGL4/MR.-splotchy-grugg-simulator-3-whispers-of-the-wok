using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeSpawn : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float startSpeed = 2;
    [SerializeField] private InputActionProperty inputAction;
    
    
    void Update()
    {
        if (inputAction.action.WasPressedThisFrame())
        {
            GameObject spawnedCub = Instantiate(cubePrefab, transform.position, transform.rotation);
            Rigidbody cubeRigidbody = spawnedCub.GetComponent<Rigidbody>();
            cubeRigidbody.linearVelocity = transform.forward * startSpeed;
        }
    }
}
