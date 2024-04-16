using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBase : MonoBehaviour
{
    [Range(0, 100), Tooltip("This is the priority of the goal; higher number = higher priority")]
    public int priority;
    
    [Tooltip("This is the unique name of the goal; used to identify the goal")]
    public string uniqueName = "Goal name";
    
    [Tooltip("Amount of time the goal hasn't been running")]
    public float idleTime;
    
    [Tooltip("Time the goal has to remain idle before it can run again")]
    public float idleTimeThreshold = 10f;

    [Tooltip("Determines if the idle time threshold has been reached")]
    public bool idleThresholdReached;
    
    [HideInInspector] public ActionBase activeAction;                                                                   // this is the action that is currently being run by the agent
    [HideInInspector] public GoapAgent agent;                                                                           // this is a reference to the agent
    
    public virtual void Start()                                                                                         // this is a function that is called by the agent when it starts
    {
        idleTime = idleTimeThreshold;
        agent = GetComponent<GoapAgent>();
    }

    public virtual void Update()
    {
        if (!idleThresholdReached)
        {
            idleTime += Time.deltaTime;
        }

        if (idleTime >= idleTimeThreshold)
        {
            idleThresholdReached = true;
        }
    }

    public virtual void UpdatePriority()                                                                                // this is a function that is called by the agent when it updates the priority of the goal
    {
        priority -= (int)Time.deltaTime;
    }

    public virtual bool CanRun()                                                                                        // this is a function that is called by the agent when it determines if the goal can be run
    {
        // if the agent is not tired and the goal has not been run in a while
        if (agent.currentStamina > 0 && idleThresholdReached)
        {
            return true;
        }
        
        agent._activeGoal = null;
        return false;
    }

    public virtual void RunGoal()                                                                                       // this is a function that is called by the agent when it runs the goal
    {
        activeAction.RunAction();
    }

    public virtual void ChangeAction(ActionBase newAction)                                                              // this is a function that is called by the agent when it changes the action
    {
        if (activeAction != null)
            activeAction.SleepAction();
            
        activeAction = newAction;
        activeAction.ResetAction();
    }

    public virtual void WakeUp()                                                                                        // this is a function that is called by the agent when it wakes up the goal
    {
        idleTime = 0f;
        activeAction.WakeAction();
    }

    public virtual void GoToSleep()                                                                                     // this is a function that is called by the agent when it puts the goal to sleep
    {
        idleThresholdReached = false;
        activeAction.SleepAction();
    }
}
