using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyState;

public class CheckState : DecoratorNode
{
    public EnemyState.State enemyState;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.currentState == enemyState)
        {
            child.Update();
            return State.Running;
        }

        return State.Failure;
    }
}
