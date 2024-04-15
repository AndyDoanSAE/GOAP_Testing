using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionWander : ActionBase
{
    private Vector3 _startingLocation;
    private Vector3 _targetLocation;

    public float currentStamina;
    public float maxStamina = 50f;
    public float staminaCost = 2f;
    public float staminaRegen = 1f;
    
    public float destinationThreshold = 0.1f;
    public float moveSpeed = 1f;
    public float wanderRadius = 10f;
    
    public override void WakeUp()
    {
        currentStamina = maxStamina;
        ResetAction();
    }

    public override void SleepAction()
    {
        
    }

    public override void ResetAction()
    {
        _startingLocation = transform.position;
        _targetLocation = _startingLocation + Random.insideUnitSphere * wanderRadius;
    }

    public override void RunAction()
    {
        if (currentStamina > 0)
        {
            currentStamina -= staminaCost * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetLocation, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _targetLocation) <= destinationThreshold)
                ResetAction();
        }
        else
        {
            currentStamina += staminaRegen * Time.deltaTime;
        }

    }
}
