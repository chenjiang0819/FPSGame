using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LowHealth : MonoBehaviour
{
    public Material material;
    public float threshold = 20f;
    public float duration = .3f;

    HealthSystem playerHealth;

    float timer = 0f;

    private void Start()
    {
        Camera cam = GetComponent<Camera>();
        var player = SceneManager.Instance.player;
        if (!player) return;
        playerHealth = player.GetComponent<HealthSystem>();
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (!playerHealth)
        {
            Graphics.Blit(src, dest);
            return;
        }

        if (playerHealth.GetCurrentHealth() < threshold)
        {
            if (timer < duration)
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

        material.SetFloat("_Intensity", Mathf.Lerp(0, 1, timer / duration));
        Graphics.Blit(src, dest, material);
    }
}
