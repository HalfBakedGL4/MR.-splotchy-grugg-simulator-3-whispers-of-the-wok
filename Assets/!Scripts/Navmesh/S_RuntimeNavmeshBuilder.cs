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

    public void BuildNavmesh()
    {
        Debug.Log("[NavmeshSurface] in BuildNavMesh() ...");
        StartCoroutine(BuildNavmeshRoutine());
    }

    public IEnumerator BuildNavmeshRoutine()
    {
        Debug.Log("[NavmeshSurface] in BuildNavmeshRoutine() ...");
        yield return new WaitForEndOfFrame();
        navmeshSurface.BuildNavMesh();
    }
}
