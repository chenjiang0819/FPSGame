using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform pivot;
    public float startOffset = 10f;
    public float speed = 1f;
    public float radiusAroundPlayer = 10f;
    public float laserTime = 3f;
    public float pauseTime = 5f;
    public AnimationCurve laserCurve;
    public AnimationCurve pauseCurve;

    Laser laser;
    Vector3 direction;
    GameObject player;
    RaycastHit hitInfo;
    LayerMask environmentLayer;

    Vector2 randomPos;
    Vector2 playerPos;
    Vector3 startPos;
    Vector3 finishPos;
    Vector3 prevFinishPos;

    float laserTimer;
    float pauseTimer;

    bool pauseResetFlag = true;
    bool laserResetFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<Laser>();
        player = SceneManager.Instance.player;
        environmentLayer = LayerMask.GetMask("Environment");

        direction = Vector3.Normalize(player.transform.position + Vector3.up - pivot.position);
        laser.start.position = pivot.position + direction * startOffset;
        laser.end.transform.position = RandomPositionAroundPlayer();

        laserTimer = laserTime;
        pauseTimer = pauseTime;
        prevFinishPos = laser.end.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // update laser start position
        direction = Vector3.Normalize(player.transform.position + Vector3.up - pivot.position);
        laser.start.position = pivot.position + direction * startOffset;

        if (pauseTimer > 0f)
        {
            if (pauseResetFlag)
            {
                startPos = RandomPositionAroundPlayer();
                pauseResetFlag = false;
            }

            pauseTimer -= Time.deltaTime;
            laser.end.transform.position = Vector3.Lerp(prevFinishPos, startPos, pauseCurve.Evaluate(1f - pauseTimer / pauseTime));
            return;
        }

        if (laserTimer > 0f)
        {
            if (laserResetFlag)
            {
                finishPos = player.transform.position + (player.transform.position - startPos);
                finishPos.y = 0;
                laserResetFlag = false;
            }

            laser.end.transform.position = Vector3.Lerp(startPos, finishPos, laserCurve.Evaluate(1f - laserTimer / laserTime));
            laserTimer -= Time.deltaTime;
        }
        else
        {
            laserTimer = laserTime;
            pauseTimer = pauseTime;
            prevFinishPos = finishPos;
            laserResetFlag = true;
            pauseResetFlag = true;
        }
    }

    private Vector3 RandomPositionAroundPlayer()
    {
        playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        randomPos = Random.insideUnitCircle * radiusAroundPlayer + playerPos;
        return new Vector3(randomPos.x, 0, randomPos.y);
    }
}
