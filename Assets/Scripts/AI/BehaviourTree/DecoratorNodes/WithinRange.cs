using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithinRange : DecoratorNode
{
    public float radius = 10f;
    public bool flip = false;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.player == null)
        {
            return State.Failure;
        }

        if ((context.transform.position - blackboard.player.transform.position).magnitude > radius)
        {
            if (flip)
            {
                child.Update();
                return State.Success;
            }
            return State.Failure;
        }
        else
        {
            if (flip)
            {
                return State.Failure;
            }
            child.Update();
            return State.Success;
        }
    }
}
