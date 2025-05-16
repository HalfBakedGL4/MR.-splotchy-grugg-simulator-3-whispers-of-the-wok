using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CustomerStateManager : MonoBehaviour
{
    public S_State currentState;

    private void Update() 
    {
        RunStateMachine();
    }

    private void RunStateMachine() 
    {
        S_State nextState = currentState?.RunCurrentState();

        if (nextState != null) 
        {
            Debug.Log("Customer changed state to: " + nextState.name);
            nextState.enabled = true;
            SwitchToTheNextState(nextState);
        }
    }

    private void SwitchToTheNextState(S_State nextState) 
    {
        currentState.enabled = false;
        currentState = nextState;
    }
}
