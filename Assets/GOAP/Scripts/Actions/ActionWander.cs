using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionWander : ActionBase
{
    private Vector3 _targetLocation;                                                                                    // the location the agent is trying to move to
    
    [Header("Wander Settings")]
    [SerializeField, Tooltip("The stamina cost of choosing a new target location.")]
    private float staminaWaitingRegen = 1f;
    [SerializeField, Tooltip("The distance the agent needs to be from the target location before it chooses a new target.")]
    private float destinationThreshold = 0.1f;
    [SerializeField, Tooltip("The radius the agent can wander in.")]
    private float wanderRadius = 10f;
    [SerializeField, Tooltip("The minimum time the agent waits before choosing a new target location.")] 
    private float minWaitTime = 1f;
    [SerializeField, Tooltip("The maximum time the agent waits before choosing a new target location.")]
    private float maxWaitTime = 3f;
    [SerializeField, Tooltip("Has the agent arrived at the target location yet?")]
    private bool hasArrivedAtDestination;
    [SerializeField, Tooltip("Is the agent choosing a new target location yet?")]
    private bool isChoosingDestination;

    public override void Start()
    {
        base.Start();
        _targetLocation = new Vector3(Random.Range(-wanderRadius, wanderRadius),transform.position.y ,Random.Range(-wanderRadius, wanderRadius));
        hasArrivedAtDestination = false;
        isChoosingDestination = false;
    }
    
    public override void ResetAction()
    {
        // reset the action
        base.ResetAction();
        StartCoroutine(ChooseNewDestination(Random.Range(minWaitTime, maxWaitTime)));
    }

    public override void RunAction()
    {
        // recover stamina while choosing a new destination
        agent.currentStamina += staminaWaitingRegen * Time.deltaTime;
        
        // if the agent is not at the target location, move towards it
        if (!hasArrivedAtDestination)
        {
            base.RunAction();
            isChoosingDestination = false;
            transform.position = Vector3.MoveTowards(transform.position, _targetLocation, agent.moveSpeed * Time.deltaTime);
        }

        // if the agent has reached the target location, reset the action
        if (Vector3.Distance(transform.position, _targetLocation) <= destinationThreshold)
        {
            hasArrivedAtDestination = true;

            if (!isChoosingDestination)
            {
                isChoosingDestination = true;
                ResetAction();
            }
        }
    }

    private IEnumerator ChooseNewDestination(float waitTime)                                                            // this is a coroutine that is called by the agent when it needs to choose a new destination
    {
        // wait for the specified time
        yield return new WaitForSeconds(waitTime);

        // choose a new destination
        _targetLocation = new Vector3(Random.Range(-wanderRadius, wanderRadius),transform.position.y ,Random.Range(-wanderRadius, wanderRadius));
        hasArrivedAtDestination = false;
        Debug.Log("New destination: " + _targetLocation);
    }
}
