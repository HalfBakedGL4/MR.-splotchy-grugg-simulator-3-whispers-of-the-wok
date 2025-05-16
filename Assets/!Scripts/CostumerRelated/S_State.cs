using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class S_State : NetworkBehaviour
{
    public abstract S_State RunCurrentState();
}
