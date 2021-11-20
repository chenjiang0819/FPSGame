using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float positionDuration = 1f;
    public float rotationDuration = .5f;
    public float positionAmount = 1f;
    public float rotationAmount = 5f;
    public AnimationCurve curve;

    Vector3 originalPos;
    Quaternion originalRot;

    float positionTimer = -1f;
    float rotationTimer = -1f;

    Vector3 randomPos;
    Quaternion randomRot;
    Vector3 prevPos;
    Quaternion prevRot;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (positionTimer < 0f)
        {
            randomPos = Random.insideUnitSphere * positionAmount + originalPos;
            prevPos = transform.position;
            positionTimer = positionDuration;
        }
        else
        {
            transform.position = Vector3.Lerp(prevPos, randomPos, curve.Evaluate(1f - positionTimer / positionDuration));
            positionTimer -= Time.deltaTime;
        }

        if (rotationTimer < 0f)
        {
            randomRot = Quaternion.Euler(originalRot.eulerAngles.x + Random.Range(-rotationAmount, rotationAmount), originalRot.eulerAngles.y + Random.Range(-rotationAmount, rotationAmount), originalRot.eulerAngles.z + Random.Range(-rotationAmount, rotationAmount));
            prevRot = transform.rotation;
            rotationTimer = rotationDuration;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(prevRot, randomRot, curve.Evaluate(1f - rotationTimer / rotationDuration));
            rotationTimer -= Time.deltaTime;
        }
    }
}
