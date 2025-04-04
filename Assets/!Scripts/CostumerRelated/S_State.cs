using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class S_State : MonoBehaviour
{
    public abstract S_State RunCurrentState();
}
