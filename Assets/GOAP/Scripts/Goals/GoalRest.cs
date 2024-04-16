using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalRest : GoalBase
{
    public override void WakeUp()
    {
        base.WakeUp();
    }
    
    
    public override void UpdatePriority()
    {
        if (agent.currentStamina < agent.maxStamina)
        {
            priority = Mathf.RoundToInt(100 - agent.currentStamina / agent.maxStamina * 100);
        }
    }

    public override bool CanRun()
    {
        return agent.currentStamina == 0 || idleThresholdReached;
    }

    public override void GoToSleep()
    {
        base.GoToSleep();
    }
}
