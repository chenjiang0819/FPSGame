using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyType : DecoratorNode
{
    public EnemyType.Type enemyType;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.enemyType == enemyType)
        {
            child.Update();
            return State.Running;
        }

        return State.Failure;
    }
}
