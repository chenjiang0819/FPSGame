using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private static float hardFactor = .8f;
    private static float normalFactor = 0.6f;
    private static float easyFactor = 0.4f;

    public float maxHealth = 100f;
    public bool gameEnded = false;

    float currentHealth;
    bool isPlayer;
    HeadshotIndicator headshotIndicator;

    private void Start()
    {
        currentHealth = maxHealth;
        isPlayer = GetComponent<PlayerController>() ? true : false;
        headshotIndicator = SceneManager.Instance.headshotIndicator;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(float health)
    {
        currentHealth = health;
    }

    public void TakeDamage(float minDamage, float maxDamage, float criticalRate, bool shotByPlayer = false, string tag = "")
    {
        if (gameEnded) return;
        if (shotByPlayer == isPlayer) return;

        // pick a random float between min and max damage
        // multiple the damage by 2 if this is a critical hit
        float damage = Random.Range(minDamage, maxDamage);
        bool isCritical = Random.Range(0f, 0.99f) < criticalRate ? true : false;

        damage = isCritical ? damage * 2 : damage;

        // check if we hit the enemy's head
        if (!string.IsNullOrEmpty(tag) && tag.CompareTo("EnemyHead") == 0)
        {
            damage *= 3;
            print("Head shot!");
            headshotIndicator.IsHeadshot();
        }

        damage = ApplyDifficulty(damage);

        currentHealth -= damage;
        if (shotByPlayer)
            print("Damage: " + damage + ", current health: " + currentHealth);

        AlertOthers();
        CheckIsDead();
    }

    private float ApplyDifficulty(float rawDamage)
    {
        if (isPlayer)
        {
            switch (GameManager.Instance.Difficulty)
            {
                case 0:
                    return rawDamage * easyFactor;
                case 1:
                    return rawDamage * normalFactor;
                case 2:
                    return rawDamage * hardFactor;
                default:
                    return rawDamage;
            }
        }
        else
        {
            switch (GameManager.Instance.Difficulty)
            {
                case 0:
                    return rawDamage * hardFactor;
                case 1:
                    return rawDamage * normalFactor;
                case 2:
                    return rawDamage * easyFactor;
                default:
                    return rawDamage;
            }
        }
    }

    private void CheckIsDead()
    {
        if (currentHealth <= 0f)
            gameObject.SendMessage("IsDead", SendMessageOptions.RequireReceiver);
    }

    private void AlertOthers()
    {
        // if this gameObject is an enemy
        // set its state to ATTACK and alert the enemies around it
        if (!isPlayer)
        {
            var aISensor = GetComponent<AISensor>();
            if (aISensor && !aISensor.IsAttacking())
                aISensor.AttackAndAlert();
        }
    }
}
