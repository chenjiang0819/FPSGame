using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : ActionNode
{
    public float duration = 1f;
    public float randomRange = 0f;
    float startTime;
    float randomDuration;

    protected override void OnStart()
    {
        startTime = Time.time;
        randomDuration = Random.Range(duration - randomRange, duration + randomRange);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Time.time - startTime > randomDuration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
