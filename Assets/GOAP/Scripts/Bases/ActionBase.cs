using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    private void Start()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        throw new NotImplementedException();
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
