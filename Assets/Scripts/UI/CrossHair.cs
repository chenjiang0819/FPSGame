using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public float spacing = 12f;
    public float size = 2f;
    public float movingSpacing = 15f;
    public float smooth = .5f;

    public float aimTime = .3f;
    public AnimationCurve curve;

    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    PlayerController controller;
    float timer = 0f;
    float sizeRatio;
    float speed;
    float prevSpeed = 0f;
    float totalSpacing;

    RectTransform pivot;

    // Start is called before the first frame update
    void Start()
    {
        controller = SceneManager.Instance.player.GetComponent<PlayerController>();
        pivot = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.IsAiming)
        {
            if (timer < aimTime)
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
        }

        sizeRatio = curve.Evaluate(Mathf.Clamp(timer / aimTime, 0.0001f, 0.9999f));

        speed = Vector3.Magnitude(controller.MovementInputRaw);
        prevSpeed = Mathf.Lerp(prevSpeed, speed, Time.deltaTime * smooth);
        totalSpacing = Mathf.Lerp(spacing, movingSpacing, prevSpeed);

        // pivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(45, 0, ratio));

        top.localPosition = new Vector3(0, Mathf.Lerp(0, totalSpacing, sizeRatio), 0);
        top.sizeDelta = new Vector2(size, Mathf.Lerp(size, 10, sizeRatio));

        bottom.localPosition = new Vector3(0, Mathf.Lerp(-0, -totalSpacing, sizeRatio), 0);
        bottom.sizeDelta = new Vector2(size, Mathf.Lerp(size, 10, sizeRatio));

        left.localPosition = new Vector3(Mathf.Lerp(-0, -totalSpacing, sizeRatio), 0, 0);
        left.sizeDelta = new Vector2(Mathf.Lerp(size, 10, sizeRatio), size);

        right.localPosition = new Vector3(Mathf.Lerp(0, totalSpacing, sizeRatio), 0, 0);
        right.sizeDelta = new Vector2(Mathf.Lerp(size, 10, sizeRatio), size);
    }
}
