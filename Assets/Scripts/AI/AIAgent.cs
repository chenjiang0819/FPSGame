using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType;
using EnemyState;

public class AIAgent : MonoBehaviour
{
    public State currentState;
    public Type enemyType;
    public GameObject deathVFX;
    public GameObject explosionSoundEffect;
    public Transform vFXPosition;
    [HideInInspector] public GameObject player;

    KillEnemyEffect killEnemyEffect;

    private void Start()
    {
        player = SceneManager.Instance.player;
        killEnemyEffect = SceneManager.Instance.postProcess.GetComponent<KillEnemyEffect>();
    }

    public void StartAttack()
    {
        switch (enemyType)
        {
            case Type.Suicide:
                gameObject.SendMessage("SuicideAttack");
                break;
            default:
                var gun = GetComponentInChildren<EnemyGun>();
                gun.WaitAndFire();
                break;
        }
    }

    public void StopAttack()
    {
        switch (enemyType)
        {
            case Type.Suicide:
                break;
            default:
                var gun = GetComponentInChildren<EnemyGun>();
                gun.StopFiring();
                break;
        }
    }

    public void IsDead()
    {
        if (killEnemyEffect) killEnemyEffect.EnemyKilled();

        Instantiate(deathVFX, vFXPosition.position, Quaternion.identity);
        Instantiate(explosionSoundEffect, vFXPosition.position, Quaternion.identity);
        switch (enemyType)
        {
            case Type.Suicide:
                gameObject.SendMessage("Explode");
                break;
            case Type.Close:
            case Type.Mid:
            case Type.Far:
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
