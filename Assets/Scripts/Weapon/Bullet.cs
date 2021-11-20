using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletImpact;
    public GameObject SoundEffect;

    float minDamage;
    float maxDamage;
    float criticalRate;
    Vector3 prevPosition;
    Vector3 curPosition;
    RaycastHit hitInfo;
    bool shotByPlayer;
    int layers;

    // Start is called before the first frame update
    void Start()
    {
        // record the original position to perform the raycast
        prevPosition = transform.position;
        layers = LayerMask.GetMask("EnemyBody")
               | LayerMask.GetMask("Environment")
               | LayerMask.GetMask("Player");

        Instantiate(SoundEffect, transform.position, Quaternion.identity);

        // set the lifespan of the bullet to 15s
        Destroy(gameObject, 15);
    }

    // Update is called once per frame
    void Update()
    {
        // perform a raycast between bullet's position at
        // current frame and position at previous frame
        curPosition = transform.position;

        if (Physics.Linecast(prevPosition, curPosition, out hitInfo, layers))
        {
            var health = hitInfo.collider.GetComponentInParent<HealthSystem>();
            if (health) health.TakeDamage(minDamage, maxDamage, 0, shotByPlayer, hitInfo.collider.gameObject.tag);

            Instantiate(bulletImpact, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));

            Destroy(gameObject);
        }

        prevPosition = curPosition;
    }

    public void SetProperties(float minDamage, float maxDamage, float criticalRate, bool shotByPlayer)
    {
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.criticalRate = criticalRate;
        this.shotByPlayer = shotByPlayer;
    }
}
