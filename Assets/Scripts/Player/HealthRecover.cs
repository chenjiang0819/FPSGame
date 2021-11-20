using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class HealthRecover : MonoBehaviour
{
    public float recoverSpeed = 5f;
    public float pauseTime = 3f;
    HealthSystem health;


    bool beingAttacked = false;
    float timer;
    float prevHealth;
    float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<HealthSystem>();
        timer = pauseTime;
        prevHealth = health.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = health.GetCurrentHealth();

        if (prevHealth > currentHealth)
        {
            beingAttacked = true;
            timer = pauseTime;
        }

        if (beingAttacked)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                beingAttacked = false;
                timer = pauseTime;
            }
        }
        else
        {
            if (currentHealth < health.maxHealth)
            {
                health.SetCurrentHealth(currentHealth + Time.deltaTime * recoverSpeed * currentHealth);
            }
        }

        prevHealth = currentHealth;
    }
}
