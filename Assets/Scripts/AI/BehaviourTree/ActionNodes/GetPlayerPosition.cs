using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetPlayerPosition : ActionNode
{
    public bool aroundPlayer = true;
    public float distanceToPlayer = 5f;
    public float randomRange = 3f;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // skip if is still reaching the last destination
        if (!blackboard.reachedDestination) return State.Success;

        if (blackboard.player == null) return State.Failure;

        blackboard.reachedDestination = false;

        if (!aroundPlayer)
        {
            blackboard.moveToPosition = blackboard.player.transform.position;
        }
        else
        {
            float randomDistance = Random.Range(distanceToPlayer - randomRange, distanceToPlayer + randomRange);
            Vector3 randomDirection = Random.onUnitSphere * randomDistance;
            randomDirection += blackboard.player.transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, randomDistance, 1))
            {
                blackboard.moveToPosition = hit.position;
            }
            else
            {
                blackboard.moveToPosition = blackboard.player.transform.position;
            }
        }

        return State.Success;
    }
}
