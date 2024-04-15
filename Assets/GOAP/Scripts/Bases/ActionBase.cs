using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    private void Start()
    {
        // get components if needed
    }

    protected virtual void WakeUp()
    {
        
    }

    protected virtual void SleepAction()
    {
        
    }

    protected virtual void ResetAction()
    {
        
    }

    protected virtual void RunAction()
    {
        
    }

    protected virtual int ActionCost()
    {
        return 0;
    }
    
    
}
