using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wander : GAction
{
    [SerializeField] private Vector3 moveRange;
    [SerializeField] private Vector3 offsetPos;
    [SerializeField] private bool isWaiting;
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;
    [SerializeField] private bool canWander = false;
    
    private Vector3 lastPos;
    
    public override bool PrePerform()
    {
        // If there is no target or target tag, return true to allow the action to be performed
        if (target == null && string.IsNullOrEmpty(targetTag))
            return true;

        // If there is a target tag, find the target GameObject
        if (!string.IsNullOrEmpty(targetTag))
            target = GameObject.FindWithTag(targetTag);

        // If there is a target, return true to allow the action to be performed
        return target != null;
    }

    public override bool PostPerform()
    {
        canWander = true;
        return true;
    }

    void Update()
    {
        if (canWander)
            Patrol();
    }

    // Gets the agent to walk to its destination
    private void Patrol()
    {
        if (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            return;
        }
        StartCoroutine(NewPos());
    }

    // Sets a new path position
    private IEnumerator NewPos()
    {
        if (!isWaiting)
        {
            isWaiting = true;
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
            lastPos = offsetPos + new Vector3(Random.Range(-moveRange.x, moveRange.x), Random.Range(-moveRange.y, moveRange.y), Random.Range(-moveRange.z, moveRange.z));
            agent.destination = lastPos;
            isWaiting = false;
        }
    }

    // Draws gizmos in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(lastPos, 1);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(offsetPos, moveRange * 2);
    }
}