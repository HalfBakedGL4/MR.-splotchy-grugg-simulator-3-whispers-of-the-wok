using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ChaseState : S_State
{
    public S_AttackState attackState;
    public bool isInAttackRange;

    public override S_State RunCurrentState() 
    {
        if (isInAttackRange) 
        {
            return attackState;
        }
        else 
        {
            return this;
        }
    }
}
