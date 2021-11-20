using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAgent : MonoBehaviour
{
    public GameObject endGame;
    public Laser laser;
    public GameObject dashObject;
    public GameObject dashHealthBar;
    public GameObject shield;
    public Mission mission;

    public float stageTransitionTime = 8f;
    public float stage1PauseTimeMin = 8f;
    public float stage1PauseTimeMax = 12f;

    CubesSpawner cubesSpawner;
    HealthSystem health;

    int prevStage = 1;
    int currentStage = 1;
    float healthRatio = 1f;
    float stageTransitionTimer;

    bool stageTransiting = false;
    bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        cubesSpawner = GetComponent<CubesSpawner>();
        health = GetComponent<HealthSystem>();

        stageTransitionTimer = stageTransitionTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStage();
        CheckIsTransioning();

        if (!stageTransiting && !attacking)
        {
            StartAttack();
            attacking = true;
        }
    }

    private void CheckIsTransioning()
    {
        if (prevStage != currentStage)
        {
            if (!stageTransiting)
            {
                stageTransiting = true;
                MoveToNextStage();
            }

            stageTransitionTimer -= Time.deltaTime;

            if (stageTransitionTimer <= 0f)
            {
                stageTransitionTimer = stageTransitionTime;
                prevStage = currentStage;
                stageTransiting = false;
            }
        }
    }

    private void MoveToNextStage()
    {
        attacking = false;
        switch (prevStage)
        {
            case 1:
                cubesSpawner.StopAttack();
                mission.UpdateMission();
                break;
            case 2:
                laser.StopAttack();
                mission.UpdateMission();
                break;
            case 3:
                print("Stage3 Attack Stopped");
                break;
            default:
                break;
        }
    }

    private void ActivateDash()
    {
        dashObject.SetActive(true);
        dashHealthBar.SetActive(true);
    }

    private void UpdateStage()
    {
        healthRatio = health.GetCurrentHealth() / health.maxHealth;

        if (healthRatio > 0.67) currentStage = 1;
        else if (healthRatio > 0.33) currentStage = 2;
        else currentStage = 3;
    }

    public void IsDead()
    {
        Destroy(GetComponent<BasicEnemySpawner>());
        endGame.SetActive(true);
    }

    private void StartAttack()
    {
        switch (currentStage)
        {
            case 1:
                cubesSpawner.StartAttack();
                Invoke("ResetStage1Attack", Random.Range(stage1PauseTimeMin, stage1PauseTimeMax));
                break;
            case 2:
                laser.gameObject.SetActive(true);
                break;
            case 3:
                shield.SetActive(true);
                Invoke("ActivateDash", 2);
                break;
            default:
                break;
        }
    }

    private void StopAttack()
    {
        attacking = false;
        switch (currentStage)
        {
            case 1:
                cubesSpawner.StopAttack();
                break;
            case 2:
                laser.StopAttack();
                break;
            case 3:
                shield.GetComponent<Shield>().DeactivateShield();
                break;
            default:
                break;
        }
    }

    private void ResetStage1Attack()
    {
        cubesSpawner.StopAttack();
        attacking = false;
    }
}
