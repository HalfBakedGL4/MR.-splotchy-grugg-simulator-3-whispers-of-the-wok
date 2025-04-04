using UnityEngine;

public class S_AttackState : S_State
{
    public S_AttackState attackState;
    public bool isInAttackRange;

    public override S_State RunCurrentState() 
    {
        /*if (isInAttackRange) 
        {
            return attackState;
        }
        else*/
        Debug.Log("I have Attacked!");
        return this;
    }
}
