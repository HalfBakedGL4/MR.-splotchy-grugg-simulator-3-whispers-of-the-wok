using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class S_LookAtPlayer : MonoBehaviour
{
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    private GameObject closestPlayer;

    private void Start()
    {
        closestPlayer = GameObject.FindGameObjectWithTag("LocalPlayer");
    }

    void Update()
    {
        if (closestPlayer == null) return;

        foreach (GameObject obj in objects)
        {
            obj.transform.LookAt(closestPlayer.transform.position);
        }
    }

}
