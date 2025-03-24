using UnityEngine;

public class S_ChaseState : S_State
{
    public override S_State RunCurrentState() 
    {
        return this;
    }
}
