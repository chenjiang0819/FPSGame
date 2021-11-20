using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyState;

public class EnemyGun : MonoBehaviour
{
    [Header("General")]
    public float turnSpeed = .5f;
    public Transform FireDirection;
    public GameObject projectile;
    public float lowestAngle = 35f;
    public bool onlyRotateAroundY = false;

    [Header("WeaponProperty")]
    public float minDamage = 10f;
    public float maxDamage = 15f;
    public float fireRate = 5f;
    public int minFireCount = 10;
    public int maxFireCount = 10;
    public float minRestTime = 5f;
    public float maxRestTime = 5f;
    public float bulletSpeed = 1000f;

    AIAgent agent;
    NavMeshAgent navMeshAgent;

    bool isFiring = false;
    bool isResting = false;

    float fireTimeInterval;
    float fireTimer = 0f;
    float restTimeInterval;
    float restTimer = 0f;
    int fireCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInParent<AIAgent>();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // adjust the gun's rotation
        AdjustRotation();

        // pause between bursts of fire
        if (isResting)
        {
            Rest();
        }

        // enemy is still firing
        if (!isResting && isFiring)
        {
            KeepFiring();
        }
    }

    private void KeepFiring()
    {
        fireTimeInterval = 1.0f / fireRate;
        fireTimer -= Time.deltaTime;
        if (fireTimer < 0)
        {
            Fire();
            fireTimer = fireTimeInterval;
            fireCount += 1;

            // pause after shooting several bullets
            if (fireCount >= maxFireCount)
            {
                isResting = true;
                restTimeInterval = Random.Range(minRestTime, maxRestTime);
                restTimer = restTimeInterval;
            }
        }
    }

    private void Rest()
    {
        restTimer -= Time.deltaTime;
        if (restTimer < 0)
        {
            isResting = false;
            fireCount = 0;
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(projectile, FireDirection.transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetProperties(minDamage, maxDamage, 0f, false);

        Vector3 direction = FireDirection.forward;
        // Vector3 direction = FireDirection.position - transform.position;
        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
    }

    private void AdjustRotation()
    {
        // point the gun towards the player if in ATTACK state
        // otherwise point towards the movement direction
        if (agent.currentState == State.Attack)
        {
            Vector3 player = agent.player.transform.position + Vector3.up * 1.4f;
            Vector3 direction = player - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            if (onlyRotateAroundY)
            {
                rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            }
            rotation = Quaternion.Euler(Mathf.Clamp(rotation.eulerAngles.x, -85, lowestAngle), rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
        }
        else
        {
            if (navMeshAgent.desiredVelocity.magnitude == 0) return;
            Vector3 desired = navMeshAgent.desiredVelocity + transform.position;
            Vector3 direction = desired - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            if (onlyRotateAroundY)
            {
                rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
        }
    }

    public void WaitAndFire()
    {
        if (isFiring) return;

        // wait a couple of seconds to prevent sudden firing
        StartCoroutine("StartFiring");
    }

    public IEnumerator StartFiring()
    {
        yield return new WaitForSeconds(Random.Range(minRestTime, maxRestTime));

        isFiring = true;
    }

    public void StopFiring()
    {
        if (!isFiring) return;
        isFiring = false;
    }
}
