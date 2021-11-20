using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public float smooth = 30f;

    HealthSystem playerHealth;
    Image image;
    Color prevColor;
    float ratio;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = SceneManager.Instance.player.GetComponent<HealthSystem>();
        image = GetComponent<Image>();
        prevColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        ratio = 1f - playerHealth.GetCurrentHealth() / playerHealth.maxHealth;
        var targetColor = image.color;
        targetColor.a = Mathf.Lerp(0, 200f / 255f, ratio);
        prevColor = Color.Lerp(prevColor, targetColor, Time.deltaTime * smooth);
        image.color = prevColor;
    }
}
