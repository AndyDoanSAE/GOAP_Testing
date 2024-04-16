using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    [Header("Main Goal Settings")]
    [Tooltip("List of goals that this action can satisfy.")]
    public List<string> canSatisfy;
    [Tooltip("How much stamina this action requires.")]
    public float staminaCost;
    
    [HideInInspector] public GoapAgent agent;                                                                           // reference to the agent
    
    public virtual void Start()                                                                                         // this is a function that is called by the agent when it starts
    {
        agent = GetComponent<GoapAgent>();
    }

    public virtual void WakeAction()                                                                                    // this is a function that is called by the agent when it wakes up
    {
        ResetAction();
    }

    public virtual void SleepAction()                                                                                   // this is a function that is called by the agent when it sleeps the action
    {
          // put the action to sleep
    }

    public virtual void ResetAction()                                                                                   // this is a function that is called by the agent when it resets the action
    {
        // reset the action
    }

    public virtual void RunAction()                                                                                     // this is a function that is called by the agent when it runs the action
    {
        // reduce stamina
        agent.currentStamina -= staminaCost * Time.deltaTime;
    }

    public virtual int ActionCost()                                                                                     // this is a function that is called by the agent when it determines the cost of the action
    {
        // return the cost of the action
        return 0;
    }
}
