using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float duration = 3f;
    public float startSize = .8f;
    public AnimationCurve colorCurve;
    public AnimationCurve sizeCurve;

    Vector3 originalScale;
    float timer = 0f;
    Material material;
    float alpha;
    bool active = true;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && timer < duration)
        {
            timer += Time.deltaTime;
            alpha = timer / duration;
            material.SetFloat("_Intensity", colorCurve.Evaluate(alpha));
            transform.localScale = Vector3.Lerp(startSize * originalScale, originalScale, sizeCurve.Evaluate(alpha));
        }

        if (!active && timer > 0f)
        {
            timer -= Time.deltaTime;
            alpha = timer / duration;
            material.SetFloat("_Intensity", colorCurve.Evaluate(alpha));
            transform.localScale = Vector3.Lerp(startSize * originalScale, originalScale, sizeCurve.Evaluate(alpha));
        }
    }

    public void DeactivateShield()
    {
        active = false;
        Destroy(gameObject, 3);
    }
}
