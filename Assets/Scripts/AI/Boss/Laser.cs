using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer laser;
    public Transform start;
    public GameObject end;
    public GameObject chargeSphere;
    public GameObject chargeVFX;
    public GameObject bigParticles;
    public GameObject moreParticles;
    public GameObject shockwave0;
    public GameObject shockwave1;

    public float damageMin = 10f;
    public float damageMax = 15f;
    public float radius = 1f;
    public float chargeTime = 3f;
    public AnimationCurve chargeCurve;

    LayerMask playerLayer;
    HealthSystem playerHealth;
    Material chargeMat;

    bool fullyCharged = false;
    bool stopAttack = false;
    Ray ray;
    float timer = 0f;

    Color chargeMatColor;
    ParticleSystem.MinMaxGradient vFXStartColor;
    Color vFXColor;
    ParticleSystem.EmissionModule bigEmission;
    ParticleSystem.EmissionModule moreEmission;
    float emissionRate;

    float alpha;

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        playerHealth = SceneManager.Instance.player.GetComponent<HealthSystem>();

        chargeMat = chargeSphere.GetComponent<Renderer>().material;
        chargeMatColor = chargeMat.GetColor("_FrontColor");
        chargeMatColor.a = 0;

        vFXStartColor = chargeVFX.GetComponent<ParticleSystem>().main.startColor;
        vFXColor = vFXStartColor.color;
        vFXColor.a = 0;
        bigEmission = bigParticles.GetComponent<ParticleSystem>().emission;
        moreEmission = moreParticles.GetComponent<ParticleSystem>().emission;
        emissionRate = bigEmission.rateOverTime.constant;

        end.SetActive(false);
        shockwave0.SetActive(false);
        shockwave1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!fullyCharged && !stopAttack && timer < chargeTime)
        {
            timer += Time.deltaTime;
            alpha = chargeCurve.Evaluate(timer / chargeTime);

            chargeMatColor.a = alpha;
            chargeMat.SetColor("_FrontColor", chargeMatColor);

            vFXColor.a = alpha;
            vFXStartColor.color = vFXColor;

            bigEmission.rateOverTime = emissionRate * alpha;
            moreEmission.rateOverTime = emissionRate * alpha;

            if (timer >= chargeTime)
            {
                fullyCharged = true;
                end.SetActive(true);
                shockwave0.SetActive(true);
                shockwave1.SetActive(true);
            }
        }

        if (fullyCharged)
        {
            UpdateLaserPosition();
            DetectCollision();
        }

        if (stopAttack && timer > 0f)
        {
            timer -= Time.deltaTime;
            alpha = chargeCurve.Evaluate(timer / chargeTime);

            chargeMatColor.a = alpha;
            chargeMat.SetColor("_FrontColor", chargeMatColor);

            vFXColor.a = alpha;
            vFXStartColor.color = vFXColor;

            bigEmission.rateOverTime = emissionRate * alpha;
            moreEmission.rateOverTime = emissionRate * alpha;
        }
    }

    public void StopAttack()
    {
        stopAttack = true;
        end.SetActive(false);
        shockwave0.SetActive(false);
        shockwave1.SetActive(false);
        laser.enabled = false;
        Invoke("StopVFXBackground", 4);
        Destroy(gameObject, 5);
    }

    private void StopVFXBackground()
    {
        chargeVFX.GetComponent<ParticleSystem>().Stop();
    }

    private void UpdateLaserPosition()
    {
        laser.SetPosition(0, start.position);
        laser.SetPosition(1, end.transform.position);
    }

    private void DetectCollision()
    {
        ray.origin = start.position;
        ray.direction = end.transform.position - start.position;
        if (Physics.SphereCast(ray, radius, 2000f, playerLayer))
        {
            playerHealth.TakeDamage(damageMin * Time.deltaTime, damageMax * Time.deltaTime, 0, false);
        }
    }
}
