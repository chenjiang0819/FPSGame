using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public GameObject vfx;
    public GameObject explosionSoundEffect;

    CubesSpawner parent;
    GameObject player;

    private void Start()
    {
        player = SceneManager.Instance.player;
        parent = SceneManager.Instance.boss.GetComponent<CubesSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13 || other.gameObject.layer == 20)
        {
            Instantiate(vfx, transform.position, Quaternion.identity);
            Instantiate(explosionSoundEffect, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
        else
        {
            return;
        }

        if (other.gameObject == player)
        {
            parent.ApplyDamage();
        }
    }
}
