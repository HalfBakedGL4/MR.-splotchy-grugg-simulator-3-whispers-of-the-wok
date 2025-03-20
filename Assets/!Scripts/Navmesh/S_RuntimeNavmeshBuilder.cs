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
        //yield return new WaitUntil(() => MRUK.Instance != null);
        MRUK.Instance.RegisterSceneLoadedCallback(BuildNavmesh);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildNavmesh()
    {
        navmeshSurface.BuildNavMesh();
    }
        /*Debug.Log("[NavmeshSurface] in BuildNavMesh() ... ");
        StartCoroutine(BuildNavmeshRoutine());
    }

    public IEnumerator BuildNavmeshRoutine()
    {
        Debug.Log("[NavmeshSurface] in BuildNavmeshRoutine() ...");
        yield return new WaitForEndOfFrame();
        navmeshSurface.BuildNavMesh();
    }*/
    }
