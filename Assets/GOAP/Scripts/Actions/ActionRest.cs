using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRest : ActionBase
{
    public override void RunAction()
    {
        agent.currentStamina += agent.staminaRegen * Time.deltaTime;

        if (agent.currentStamina >= agent.maxStamina)
            agent.currentStamina = agent.maxStamina;
    }
}
