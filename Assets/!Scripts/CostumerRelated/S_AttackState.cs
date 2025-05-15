using UnityEngine;

public class S_AttackState : S_State
{
    public S_ChaseState chaseState;
    public S_AttackState attackState;
    public bool isInAttackRange;

    public override S_State RunCurrentState() 
    {
        if (isInAttackRange) 
        {
            return attackState;
        }

        else return chaseState;
        
        
    }
}
