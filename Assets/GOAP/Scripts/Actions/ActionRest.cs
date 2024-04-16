using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRest : ActionBase
{
    public override void RunAction()
    {
        // recover stamina
        agent.currentStamina += agent.staminaRegen * Time.deltaTime;

        // if the agent's current stamina goes past the maximum, set it to the maximum
        if (agent.currentStamina >= agent.maxStamina)
            agent.currentStamina = agent.maxStamina;
    }
}
