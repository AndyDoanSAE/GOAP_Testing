using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapAgent : MonoBehaviour
{
    private List<GoalBase> _goals;
    private List<ActionBase> _actions;

    [SerializeField] public GoalBase _activeGoal;
    
    public float currentStamina;
    public float maxStamina = 50f;
    public float staminaRegen = 1f;
    public float moveSpeed = 5f;
    
    private void Start()
    {
        _goals = new List<GoalBase>(GetComponents<GoalBase>());
        _actions = new List<ActionBase>(GetComponents<ActionBase>());
        
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentStamina < 0)
            currentStamina = 0;
        
        if (currentStamina >= maxStamina)
            currentStamina = maxStamina;
        
        DetermineGoal();

        if (_activeGoal)
            _activeGoal.RunGoal();
    }

    private void DetermineGoal()
    {
        // find the best goal
        GoalBase bestGoal = null;
        ActionBase bestAction = null;

        // if the active goal is not null and can run, use it
        if (_activeGoal != null && _activeGoal.CanRun())
        {
            bestGoal = _activeGoal;
            bestAction = _activeGoal.activeAction;
        }

        // find the highest priority goal that can run
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
                
                if (bestAction != null)
                    bestGoal = goal;
            }
        }

        if (bestGoal == null)
        {
            Debug.LogError("No goal can be satisfied for " + gameObject.name);
            return;
        }

        // if the best goal is different from the active goal, change the active goal to the best goal
        if (_activeGoal != bestGoal)
        {
            // if the active goal is not null, put it to sleep
            if (_activeGoal != null)
                _activeGoal.GoToSleep();
            
            _activeGoal = bestGoal;
            _activeGoal.ChangeAction(bestAction);
            
            bestGoal.WakeUp();
        }
        else if (bestAction != bestGoal.activeAction)
        {
            // if the best action is different from the active action, change the active action to the best action
            if (bestAction.ActionCost() < bestGoal.activeAction.ActionCost())
                _activeGoal.ChangeAction(bestAction);
        }
    }
}
