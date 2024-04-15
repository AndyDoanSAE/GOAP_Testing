using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GoalBase : MonoBehaviour
{
    [Range(0, 100)]
    public int priority = 0;
    public string uniqueName = "Goal name";
    
    public ActionBase activeAction;
    
    public virtual void UpdatePriority()
    {
        // set the priority of the goal
    }

    public virtual bool CanRun()
    {
        // returns true if the goal can be run, false if not
        return true;
    }

    public virtual void RunGoal()
    {
        // runs the goal
        activeAction.RunAction();
    }

    public virtual void ChangeAction(ActionBase newAction)
    {
        // changes the action to the new action
        if (activeAction != null)
            activeAction.SleepAction();
            
        activeAction = newAction;
        activeAction.ResetAction();
    }

    public virtual void WakeUp()
    {
        // wakes up the goal
        activeAction.WakeUp();
    }

    public virtual void GoToSleep()
    {
        // puts the goal to sleep
        activeAction.SleepAction();
    }
}
