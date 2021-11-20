using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class KillEnemyEffect : MonoBehaviour
{
    public float EffectTime = .2f;
    public float cAMaxIntensity = .8f;
    public float cAMinIntensity = .2f;
    public float lDMaxIntensity = 10f;
    public float lDMinIntensity = 5f;
    public float bLMaxIntensity = 8f;
    public float bLMinIntensity = 3f;
    public AnimationCurve curve;

    ChromaticAberration chromaticAberration;
    LensDistortion lensDistortion;
    Bloom bloom;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        var volumn = GetComponent<PostProcessVolume>();
        volumn.profile.TryGetSettings(out chromaticAberration);
        volumn.profile.TryGetSettings(out lensDistortion);
        volumn.profile.TryGetSettings(out bloom);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(cAMinIntensity, cAMaxIntensity, 1f - curve.Evaluate(timer / EffectTime));
            lensDistortion.intensity.value = Mathf.Lerp(lDMinIntensity, lDMaxIntensity, 1f - curve.Evaluate(timer / EffectTime));
            bloom.intensity.value = Mathf.Lerp(bLMinIntensity, bLMaxIntensity, 1f - curve.Evaluate(timer / EffectTime));
            timer -= Time.deltaTime;
        }
    }

    public void EnemyKilled()
    {
        timer = EffectTime;
    }
}
