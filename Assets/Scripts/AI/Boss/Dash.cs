using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float horizontalRange = 5f;
    public float dashTime = .2f;
    public float pauseTime = 3f;
    public float restTime = 1f;
    public float damageTime = .1f;
    public float dissolveTime = .5f;

    public AnimationCurve dashCurve;
    public float collisionRadius = 0.6f;

    public float minDamage = 10f;
    public float maxDamage = 15f;
    public float criticalRate = .3f;

    public GameObject baseCube;
    public GameObject lowCube;
    public GameObject highCube;

    float dashTimer;
    float pauseTimer;
    float restTimer;
    float damageTimer;

    GameObject player;
    BossAgent boss;
    Vector3 direction;
    Vector3 left;
    float distance;
    float ratio;
    Vector3 target1Pos;
    Vector3 target2Pos;
    Vector3 target3Pos;
    Vector3 restPos;
    Vector3 prevPos;

    bool resting = false;
    bool pausing = false;
    bool resetFlag = false;
    bool dead = false;

    int stage = 0;

    float yPos;
    LayerMask playerLayer;
    HealthSystem playerHealth;

    Material[] materials = new Material[3];

    // Start is called before the first frame update
    void Start()
    {
        player = SceneManager.Instance.player;
        boss = SceneManager.Instance.boss.GetComponent<BossAgent>();
        dashTimer = dashTime;
        restTimer = restTime;
        pauseTimer = 0f;
        damageTimer = damageTime;

        yPos = transform.position.y;
        playerLayer = LayerMask.GetMask("Player");
        playerHealth = SceneManager.Instance.player.GetComponent<HealthSystem>();

        materials[0] = baseCube.GetComponent<Renderer>().material;
        materials[1] = lowCube.GetComponent<Renderer>().material;
        materials[2] = highCube.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) return;
        RestPauseAndMove();
        DetectCollision();
    }

    private void DetectCollision()
    {
        if (Physics.CheckSphere(transform.position, collisionRadius, playerLayer))
        {
            damageTimer -= Time.deltaTime;

            if (damageTimer <= 0f)
            {
                distance = Vector3.Magnitude(player.transform.position + Vector3.up - transform.position);
                playerHealth.TakeDamage(minDamage, maxDamage, criticalRate, false);
                damageTimer = damageTime;
            }
        }
        else
        {
            damageTimer = damageTime;
        }
    }

    public void IsDead()
    {
        if (dead) return;

        dead = true;
        GetComponentInChildren<Rotate>().SetIsDead();
        boss.shield.GetComponent<Shield>().DeactivateShield();
        StartCoroutine("Dissolve");
    }

    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(.5f);

        for (float time = 0; time < dissolveTime; time += Time.deltaTime)
        {

            materials[0].SetFloat("_SliceAmount", time / dissolveTime);
            materials[1].SetFloat("_SliceAmount", time / dissolveTime);
            materials[2].SetFloat("_SliceAmount", time / dissolveTime);

            yield return null;
        }
        Destroy(gameObject);
    }

    private void RestPauseAndMove()
    {
        if (resting)
        {
            restTimer -= Time.deltaTime;

            if (restTimer <= 0f)
            {
                resting = false;
                restTimer = restTime;
            }
        }

        if (pausing)
        {
            pauseTimer -= Time.deltaTime;

            if (pauseTimer <= 0f)
            {
                pausing = false;
                pauseTimer = pauseTime;
            }
        }

        if (pausing || resting) return;

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            dashTimer = dashTime;
            resetFlag = true;

            if (stage == 3)
            {
                FindRestPos();
                pausing = true;
                stage = 4;
            }
            else if (stage == 4)
            {
                resting = true;
                stage = 1;
            }
            else
            {
                pausing = true;
                stage++;
            }

            prevPos = transform.position;
        }
        else
        {
            if (resetFlag)
            {
                SetTargetPositions();
                resetFlag = false;
            }
            MoveToTarget();
        }
    }

    private void FindRestPos()
    {
        float dist = Random.Range(30, 40);
        float angle = Random.Range(0, 360);
        float angleRadians = angle * Mathf.PI / 180f;

        float x = Mathf.Cos(angleRadians) * dist;
        float z = Mathf.Sin(angleRadians) * dist;

        restPos = new Vector3(x, 0, z) + player.transform.position;
        restPos.y = yPos;
    }

    private void MoveToTarget()
    {
        float delta = dashCurve.Evaluate(1f - dashTimer / dashTime);

        if (stage == 1)
            transform.position = Vector3.Lerp(prevPos, target1Pos, delta);
        if (stage == 2)
            transform.position = Vector3.Lerp(prevPos, target2Pos, delta);
        if (stage == 3)
            transform.position = Vector3.Lerp(prevPos, target3Pos, delta);
        if (stage == 4)
            transform.position = Vector3.Lerp(prevPos, restPos, delta);
    }

    private void SetTargetPositions()
    {
        direction = player.transform.position - transform.position;
        distance = Vector3.Magnitude(direction);
        direction = Vector3.Normalize(direction);
        left = Vector3.Cross(direction, Vector3.up).normalized;

        target1Pos = transform.position + direction * distance / 3 + left * horizontalRange;
        target2Pos = transform.position + direction * distance * 2 / 3 - left * horizontalRange;
        target3Pos = player.transform.position;

        target1Pos.y = yPos;
        target2Pos.y = yPos;
        target3Pos.y = yPos;
    }
}
