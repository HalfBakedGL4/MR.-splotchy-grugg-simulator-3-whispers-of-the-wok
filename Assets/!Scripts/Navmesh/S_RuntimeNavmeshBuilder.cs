using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Meta.XR.MRUtilityKit;
using System.Collections;
using System;

public class S_RuntimeNavmeshBuilder : MonoBehaviour
{

    private NavMeshSurface navmeshSurface;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navmeshSurface = GetComponent<NavMeshSurface>();
        MRUK.Instance.RegisterSceneLoadedCallback(BuildNavmesh);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildNavmesh()
    {
        StartCoroutine(BuildNavmeshRoutine());
    }

    public IEnumerator BuildNavmeshRoutine()
    {
        yield return new WaitForEndOfFrame();
        navmeshSurface.BuildNavMesh();
    }
}
