using Oculus.Interaction.Surfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class S_ChaseState : S_State
{
    public S_Alien movement;
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

    private void Start()
    {
        movement.enabled = true;
    }
}
