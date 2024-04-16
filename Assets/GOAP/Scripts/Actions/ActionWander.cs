using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWander : ActionBase
{
    private Vector3 _targetLocation;    // the location the agent is trying to move to

    [SerializeField, Tooltip("The distance the agent needs to be from the target location before it chooses a new target")]
    private float destinationThreshold = 0.1f;

    [SerializeField, Tooltip("The radius the agent can wander in")]
    private float wanderRadius = 10f;
    
    [SerializeField, Tooltip("The minimum time the agent waits before choosing a new target location")] 
    private float minWaitTime = 1f;
    
    [SerializeField, Tooltip("The maximum time the agent waits before choosing a new target location")]
    private float maxWaitTime = 3f;

    public override void ResetAction()
    {
        _targetLocation = new Vector3(Random.Range(-wanderRadius, wanderRadius),transform.position.y ,Random.Range(-wanderRadius, wanderRadius));
        Debug.Log("New target location: " + _targetLocation);
    }

    public override void RunAction()
    {
        base.RunAction();
        transform.position = Vector3.MoveTowards(transform.position, _targetLocation, agent.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetLocation) <= destinationThreshold)
        {
            Invoke(nameof(ResetAction), Random.Range(minWaitTime, maxWaitTime));
        }
    }
}
