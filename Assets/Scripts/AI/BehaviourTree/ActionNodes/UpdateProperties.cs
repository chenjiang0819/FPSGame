using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateProperties : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.aIAgent)
        {
            blackboard.currentState = context.aIAgent.currentState;
            blackboard.enemyType = context.aIAgent.enemyType;
            blackboard.player = context.aIAgent.player;
            return State.Running;
        }

        return State.Failure;
    }
}
