using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotateScale : MonoBehaviour
{
    HealthSystem health;
    Rotate rotate;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponentInParent<HealthSystem>();
        rotate = GetComponent<Rotate>();
    }

    // Update is called once per frame
    void Update()
    {
        rotate.scale = 1.05f - health.GetCurrentHealth() / health.maxHealth;
        if (health.GetCurrentHealth() <= 0f) rotate.SetIsDead();
    }
}
