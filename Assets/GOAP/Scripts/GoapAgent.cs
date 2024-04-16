using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GoapAgent : MonoBehaviour
{
    private List<GoalBase> _goals;                                                                                      // list of all goals
    private List<ActionBase> _actions;                                                                                  // list of all actions

    [Tooltip("The current active goal.")] 
    public GoalBase activeGoal;
    
    [Header("Movement Settings")]
    [Tooltip("The current stamina of the agent.")]
    public float currentStamina;
    [Tooltip("The maximum stamina of the agent.")]
    public float maxStamina = 50f;
    [Tooltip("How much stamina the agent regains every second.")]
    public float staminaRegen = 1f;
    [Tooltip("How fast the agent moves.")]
    public float moveSpeed = 5f;
    
    private void Start()
    {
        _goals = new List<GoalBase>(GetComponents<GoalBase>());
        _actions = new List<ActionBase>(GetComponents<ActionBase>());
        
        currentStamina = maxStamina;
    }
    
    private void Update()
    {
        // if the current stamina goes below 0, set it to 0
        if (currentStamina < 0)
            currentStamina = 0;
        
        // if the current stamina goes past the maximum, set it to the maximum
        if (currentStamina >= maxStamina)
            currentStamina = maxStamina;
        
        DetermineGoal();

        // if there is an active goal, run it
        if (activeGoal)
            activeGoal.RunGoal();
    }

    private void DetermineGoal()
    {
        // find the best goal
        GoalBase bestGoal = null;
        ActionBase bestAction = null;

        // if there is an active goal and it can run, use it
        if (activeGoal != null && activeGoal.CanRun())
        {
            bestGoal = activeGoal;
            bestAction = activeGoal.activeAction;
        }

        // loop through all the goals and find the highest priority goal that can run
        foreach (var goal in _goals)
        {
            // update the goal priority
            goal.UpdatePriority();
            
            // if the goal can't run, skip it
            if (!goal.CanRun())
                continue;
            
            // if the goal is better than the current best goal, set it as the best goal
            if (bestGoal == null || goal.priority > bestGoal.priority)
            {
                bestAction = null;
                
                // loop through all the actions and find the one that can satisfy the goal
                foreach (var action in _actions)
                {
                    // if the action can satisfy the goal, set it as the best action
                    if (action.canSatisfy.Contains(goal.uniqueName))
                    {
                        if (bestAction == null || action.ActionCost() < bestAction.ActionCost())
                        {
                            bestAction = action;
                            break;
                        }
                    }
                }
                
                // if there is a best action, set the best goal to the current goal
                if (bestAction != null)
                    bestGoal = goal;
            }
        }

        // if there is no best goal, log an error and return
        if (bestGoal == null)
        {
            Debug.LogError("No goal can be satisfied for " + gameObject.name);
            return;
        }

        // if the best goal is different from the active goal, change the active goal to the best goal
        if (activeGoal != bestGoal)
        {
            // if there is an active goal, put it to sleep
            if (activeGoal != null)
                activeGoal.GoToSleep();
            
            // set the active goal to the new best goal and change the active action to the best action
            activeGoal = bestGoal;
            activeGoal.ChangeAction(bestAction);
            
            // wake up the best goal
            bestGoal.WakeUp();
        }
        // else if the best goal is the same as the active goal, but the best action is not the current active action, change the active action to the best action
        else if (bestAction != bestGoal.activeAction)
        {
            // if the cost of the best action is less than the cost of the current active action, change the active action to the best action (for when a goal has multiple actions that can satisfy it)
            if (bestAction.ActionCost() < bestGoal.activeAction.ActionCost())
                activeGoal.ChangeAction(bestAction);
        }
    }
}
