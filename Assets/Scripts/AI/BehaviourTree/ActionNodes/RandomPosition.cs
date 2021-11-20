using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPosition : ActionNode
{
    public float radius = 20f;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += context.transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            blackboard.moveToPosition = hit.position;
            return State.Success;
        }

        return State.Failure;
    }
}
