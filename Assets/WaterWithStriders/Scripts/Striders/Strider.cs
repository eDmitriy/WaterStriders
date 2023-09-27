using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Strider
{
    public bool drawDebug = false;

    public float moveSpeed = 1f;
    public float rotationSpeed = 1f;

    [SerializeField]
    private Vector3 currPosition = Vector3.zero;
    public Vector3 CurrPosition
    {
        get => currPosition;
        set => currPosition = value;
    }

    [SerializeField]
    private Vector3 targetPosition = Vector3.zero;
    
    private Quaternion currRotation = Quaternion.identity;
    //private Quaternion desiredRotation = Quaternion.identity;


    public bool IsNearTarget(ref float targetReachDistance)
    {
        return Vector3.Distance(CurrPosition, targetPosition)< targetReachDistance;
    }

    public void SetNewTarget(Vector3 targetPoint)
    {
        targetPosition = targetPoint;
    }

    public void Move()
    {
        Vector3 dir = (targetPosition - currPosition).normalized;
        var desiredRotation = Quaternion.LookRotation(dir);

        currRotation = Quaternion.RotateTowards(currRotation, desiredRotation, rotationSpeed);

        Vector3 forward = currRotation * Vector3.forward;
        CurrPosition += forward * moveSpeed;

        if (drawDebug)
        {
            Debug.DrawRay(CurrPosition, forward * 2, Color.cyan, 3);
            Debug.DrawRay(CurrPosition, Vector3.up * 2, Color.cyan, 3);
            Debug.DrawLine(currPosition, targetPosition, Color.red, 0.5f);
        }
    }
}
