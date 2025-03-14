using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoleSpawner : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] private GameObject holePrefab;
    [SerializeField] private float startSpeed = 2;
    [SerializeField] private InputActionProperty inputAction;


    void Update()
    {
        if (inputAction.action.WasPressedThisFrame())
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                
                Quaternion rot = Quaternion.Euler(0,hit.transform.rotation.y,0);
                GameObject spawnedCub = Instantiate(holePrefab, hit.point, hit.transform.rotation);

                
            }
        }
    }
}
