using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private List<GoalBase> _goals;
    private List<ActionBase> _actions;

    private GoalBase _activeGoal;
    
    // Start is called before the first frame update
    private void Start()
    {
        _goals = new List<GoalBase>();
        _actions = new List<ActionBase>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void DetermineGoal()
    {
        GoalBase bestGoal = null;
        ActionBase bestAction = null;

        if (_activeGoal != null && _activeGoal.CanRun())
        {
            bestGoal = _activeGoal;
            bestAction = _activeGoal.activeAction;
        }

        foreach (var g in _goals)
        {
            g.UpdatePriority();
            
            if (!g.CanRun())
                continue;
            
            if (bestGoal == null || g.priority < bestGoal.priority)
            {
                bestAction = null;

                foreach (var a in _actions)
                {
                    if (a.canSatisfy.Contains(g.uniqueName))
                    {
                        if (bestAction == null || g.priority < bestGoal.priority)
                        {
                            bestAction = a;
                            break;
                        }
                    }
                }
            }
            
            if (bestGoal != null)
                bestGoal = g;
        }

        if (bestGoal == null)
        {
            Debug.LogError("No goal can be satisfied for " + gameObject.name);
            return;
        }

        if (_activeGoal != bestGoal)
        {
            if (_activeGoal != null)
                _activeGoal.GoToSleep();
            
            _activeGoal = bestGoal;
            _activeGoal.ChangeAction(bestAction);
            
            bestGoal.WakeUp();
        }
        else if (bestAction != bestGoal.activeAction)
        {
            if (bestAction.ActionCost() < bestGoal.activeAction.ActionCost())
                _activeGoal.ChangeAction(bestAction);
        }
        
    }
}
