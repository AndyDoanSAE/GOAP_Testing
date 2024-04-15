using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}

public class GAgent : MonoBehaviour
{
    public List<GAction> action = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    private GPlanner Planner;
    private Queue<GAction> actionQueue;
    public GAction currentAction;
    private SubGoal currentGoal;
    
    protected virtual void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
            action.Add(a);
    }

    private bool invoked = false;

    private void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }
    
    private void LateUpdate()
    {
        if (currentAction != null && currentAction.running) // change when not using NavMesh
        {
            if (currentAction.target != null)
            {
                float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, transform.position);
                if (currentAction.agent.hasPath && distanceToTarget < 2f)
                {
                    if (!invoked)
                    {
                        Invoke("CompleteAction", currentAction.duration);
                        invoked = true;
                    }
                }
            }
            else
            {
                // Handle the case when there is no target
                // You can add your custom logic here
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }

            return;
        }
        
        if (Planner == null || actionQueue == null)
        {
            Planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = Planner.Plan(action, sg.Key.sgoals, null);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
                goals.Remove(currentGoal);

            Planner = null;
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                if (currentAction.targetTag != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                    // insert movement code after removing NavMesh code
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}
