using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float speed = 1f;

    float timer = 5f;

    Vector3 playerPos;
    GameObject player;

    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        player = SceneManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (target && timer > 0f)
        {
            MoveToTarget();
        }
        else
        {
            if (!started)
            {
                playerPos = player.transform.position + Vector3.up * .3f;
                started = true;
            }
            transform.position = Vector3.Lerp(transform.position, playerPos, Time.deltaTime * speed);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1, Time.deltaTime * speed);
        }

        timer -= Time.deltaTime;
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * speed);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 5, Time.deltaTime * speed);
    }
}
