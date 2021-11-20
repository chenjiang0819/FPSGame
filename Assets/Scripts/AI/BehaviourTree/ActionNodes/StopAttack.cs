using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAttack : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.aIAgent.StopAttack();
        return State.Success;
    }
}
