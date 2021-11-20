using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public bool IsRecoiling { get; set; }
    public float vertical { get; set; }
    public float horizontal { get; set; }
    public float time { get; set; }

    private float elapsedTime = 0.0f;

    private void Start()
    {
        IsRecoiling = false;
        vertical = horizontal = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRecoiling)
        {
            if (elapsedTime < time)
            {
                float xRotation = transform.eulerAngles.x - vertical;
                float yRotation = transform.eulerAngles.y + horizontal;
                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

                elapsedTime += Time.deltaTime;
                vertical *= (time - elapsedTime) / time;
                horizontal *= (time - elapsedTime) / time;
            }
            else
            {
                IsRecoiling = false;
                elapsedTime = 0.0f;
            }
        }
    }
}
