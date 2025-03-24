using UnityEngine;

public class S_IdleSate : S_State
{
    public S_ChaseState chaseState;
    public bool canSeeThePlayer;

    public override S_State RunCurrentState() 
    {
        if (canSeeThePlayer) 
        {
            return chaseState;
        }
        else 
        {
            return this;
        }
        return this;
    }
}
