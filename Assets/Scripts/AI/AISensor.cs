using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyState;

[RequireComponent(typeof(AIAgent))]
public class AISensor : MonoBehaviour
{
    public float sightDistance = 10f;
    public float alertDistance = 20f;
    [Range(1, 85)]
    public float horizontalAngle = 30f;
    public Vector3 offset = Vector3.zero;

    public int scanFrequency = 30;
    public LayerMask occlusionLayers;

    int count;
    float scanInterval;
    float scanTimer;

    bool canSeePlayer = false;
    AIAgent agent;

    Vector3 debugFeet = Vector3.zero;
    Vector3 debugChest = Vector3.zero;
    Vector3 debugHead = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<AIAgent>();
        scanInterval = 1.0f / scanFrequency;
        scanTimer = scanInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.currentState != State.Attack)
        {
            scanTimer -= Time.deltaTime;
            if (scanTimer < 0)
            {
                scanTimer += scanInterval;
                ScanPlayer();
            }
        }
    }

    private void ScanPlayer()
    {
        canSeePlayer = false;

        Vector3 origin = transform.position;
        Vector3 dest = agent.player.transform.position;
        Vector3 direction = dest - origin;

        // player is too far away
        if (direction.magnitude > sightDistance) return;

        // player is not in the front of this enemy
        direction.y = 0;
        float horizontalDelta = Vector3.Angle(direction, transform.forward);
        if (horizontalDelta > horizontalAngle) return;

        // player's head / chest / feet is blocked by obstacles
        debugFeet = dest;
        debugChest = dest + Vector3.up * .8f;
        debugHead = dest + Vector3.up * 1.6f;

        if (Physics.Linecast(dest, origin, occlusionLayers) &&  // feet position
            Physics.Linecast(dest + .8f * Vector3.up, origin, occlusionLayers) &&  // chest position
            Physics.Linecast(dest + 1.6f * Vector3.up, origin, occlusionLayers))  // head position
        {
            return;
        }

        canSeePlayer = true;
        AttackAndAlert();
    }

    public bool IsAttacking()
    {
        return agent.currentState == State.Attack;
    }

    public void AttackAndAlert()
    {
        agent.currentState = State.Attack;

        // switch all the enemies around it into ATTACK state
        Collider[] colliders = Physics.OverlapSphere(transform.position, alertDistance, LayerMask.GetMask("Enemy"));
        foreach (var c in colliders)
        {
            var agent = c.GetComponent<AIAgent>();
            agent.currentState = State.Attack;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 center = transform.position + offset.x * transform.right + offset.y * transform.up + offset.z * transform.forward;
        Vector3 right = Quaternion.Euler(0, horizontalAngle, 0) * transform.forward * sightDistance + center;
        Vector3 left = Quaternion.Euler(0, -horizontalAngle, 0) * transform.forward * sightDistance + center;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(center, right);
        Gizmos.DrawLine(center, left);
        Gizmos.DrawWireSphere(center, sightDistance);

        Gizmos.DrawSphere(debugFeet, .2f);
        Gizmos.DrawSphere(debugChest, .2f);
        Gizmos.DrawSphere(debugHead, .2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, alertDistance);

        if (canSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(agent.player.transform.position, .5f);
        }
    }
}
