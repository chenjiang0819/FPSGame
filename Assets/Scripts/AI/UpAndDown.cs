using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    public float range = .3f;
    public float speed = 2f;

    Vector3 startPosition;
    float offset;

    private void Start()
    {
        startPosition = transform.localPosition;
        offset = Random.Range(0, 100f);
    }

    void Update()
    {
        transform.localPosition = startPosition + Vector3.up * range * Mathf.Sin((Time.time + offset) * speed);
    }
}
