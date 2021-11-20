using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthDisplay : MonoBehaviour
{
    public GameObject enemy;
    HealthSystem health;
    RectTransform rect;

    float width;

    // Start is called before the first frame update
    void Start()
    {
        health = enemy.GetComponent<HealthSystem>();
        rect = GetComponent<RectTransform>();

        width = rect.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = health.GetCurrentHealth() / health.maxHealth;
        rect.sizeDelta = new Vector2(width * ratio, rect.sizeDelta.y);
    }
}
