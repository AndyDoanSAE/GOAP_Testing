using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    public List<string> canSatisfy;
    
    private void Start()
    {
        // get components if needed
    }

    public virtual void WakeUp()
    {
        // if we want the actions to reset every time they wake up uncomment below
        //ResetAction();
    }

    public virtual void SleepAction()
    {
        // puts the action in a sleeping state
    }

    public virtual void ResetAction()
    {
        // resets the action to its default state
    }

    public virtual void RunAction()
    {
        // runs the action
    }

    public virtual int ActionCost()
    {
        // returns the cost of the action
        return 0;
    }
}
