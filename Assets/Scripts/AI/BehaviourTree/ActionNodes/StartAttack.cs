using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAttack : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.aIAgent.StartAttack();
        return State.Success;
    }
}
