using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]

public class S_CuttingObjects : MonoBehaviour
{
    //float countDown = 10f;
    bool canChop = true;

    int currentChild;
    GameObject child;

    public UnityEvent OnChop;

    [Button("chop")]
    public void TryChop()
    {
        //if (canChop) ChopObject();
        ChopObject();
    }

    void Start()
    {
    }

    void ChopObject()
    {
        OnChop?.Invoke();

        Debug.Log(transform.childCount);
        Debug.Log(currentChild);
        //StartCoroutine(Timer());

        child = transform.GetChild(currentChild).gameObject;
        child.AddComponent<Rigidbody>();
        child.transform.parent = null;


        if (currentChild == 0)
        {
            Destroy(gameObject);
        }
        currentChild--;
    }

    /*IEnumerator Timer()
    {
        canChop = false;
        yield return new WaitForSeconds(countDown);
        canChop = true;
    }*/

    private void OnDestroy()
    {
        //fix networking idfk
    }
}
