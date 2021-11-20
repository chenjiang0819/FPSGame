using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadshotIndicator : MonoBehaviour
{
    public float duration = .1f;
    public Image[] children = new Image[4];

    float timer = -1f;
    float alpha;
    Color color;
    bool resetFlag = false;

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            if (!resetFlag) resetFlag = true;

            alpha = timer / duration;
            color = children[0].color;
            color.a = alpha;

            foreach (var c in children)
            {
                c.color = color;
            }

            timer -= Time.deltaTime;
        }
        else
        {
            if (resetFlag)
            {
                color.a = 0;
                foreach (var c in children)
                {
                    c.color = color;
                }
                resetFlag = false;
            }

        }
    }

    public void IsHeadshot()
    {
        timer = duration;
    }
}
