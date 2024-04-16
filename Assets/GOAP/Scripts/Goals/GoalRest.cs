using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalRest : GoalBase
{
    public override void UpdatePriority()
    {
        // change the priority based on the agent's current stamina
        if (agent.currentStamina < agent.maxStamina)
        {
            priority = Mathf.RoundToInt(100 - agent.currentStamina / agent.maxStamina * 100);
        }
    }

    public override bool CanRun()
    {
        // if the agent's current stamina is 0 or the idle threshold has been reached, the goal can be run
        return agent.currentStamina == 0 || idleThresholdReached;
    }
}
