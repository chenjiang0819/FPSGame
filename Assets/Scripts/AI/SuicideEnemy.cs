using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideEnemy : MonoBehaviour
{
    public float countDownTime = 3f;
    public float bombRadius = 5f;
    public float bombDamage = 30f;
    public float bombRandomRange = 5f;
    public LayerMask bombLayers;

    public GameObject vfx;
    public GameObject explosionSoundEffect;
    public Color deadColor;

    Material material;
    Color startColor;
    KillEnemyEffect killEnemyEffect;

    private void Start()
    {
        material = GetComponentInChildren<Renderer>().material;
        startColor = material.GetColor("_Tint");
        killEnemyEffect = SceneManager.Instance.postProcess.GetComponent<KillEnemyEffect>();
    }

    private void SuicideAttack()
    {
        // remove the BehaviourTreeRunner component
        var bTRunner = GetComponent<BehaviourTreeRunner>();
        if (bTRunner) Destroy(bTRunner);

        // explode after the count down
        StartCoroutine(CountDown());
        Invoke("Explode", countDownTime);
    }

    void Explode()
    {
        killEnemyEffect.EnemyKilled();

        // apply damages to all the enemies and player around it
        Collider[] colliders = Physics.OverlapSphere(transform.position, bombRadius, bombLayers);
        if (colliders.Length != 0)
        {
            foreach (var c in colliders)
            {
                // skip if current collider is itself
                if (GameObject.ReferenceEquals(gameObject, c.gameObject)) continue;

                // skip if the collider doesn't have a HealthSystem component
                var healthSystem = c.GetComponent<HealthSystem>();
                if (!healthSystem) continue;

                // apply damage to this collider according to the distance between them
                // the closer the collider, the greater the damage
                float distanceToCenter = (c.gameObject.transform.position - transform.position).magnitude;
                float damage = bombDamage * (1f - distanceToCenter / bombRadius);
                healthSystem.TakeDamage(damage - bombRandomRange, damage + bombRandomRange, 0, false);
            }
        }

        GameObject explosion = Instantiate(vfx, transform.position, Quaternion.identity) as GameObject;
        Instantiate(explosionSoundEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 5);
        Destroy(gameObject);
    }

    private IEnumerator CountDown()
    {
        for (float time = 0; time < countDownTime; time += Time.deltaTime)
        {
            material.SetColor("_Tint", Color.Lerp(startColor, deadColor, time / countDownTime));
            yield return null;
        }
    }
}
