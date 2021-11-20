using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemyGroup : MonoBehaviour
{
    public GameObject enemyGroup;
    GameObject player;

    private void Start()
    {
        player = SceneManager.Instance.player;
        enemyGroup.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            enemyGroup.SetActive(true);
            Destroy(gameObject);
        }
    }
}
