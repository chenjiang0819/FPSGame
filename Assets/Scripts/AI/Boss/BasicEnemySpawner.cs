using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyState;

public class BasicEnemySpawner : MonoBehaviour
{
    public GameObject[] enemyTypes;
    public float spawnRate = .2f;
    public GameObject parent;

    GameObject player;
    Transform mainCam;

    float timer;
    float timeInteval;

    int numTypes;

    // Start is called before the first frame update
    void Start()
    {
        player = SceneManager.Instance.player;
        mainCam = SceneManager.Instance.mainCam.transform;

        timeInteval = 1f / spawnRate;
        timer = timeInteval;

        numTypes = enemyTypes.Length;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            int index = Random.Range(0, numTypes);

            // spawn an enemy behind the camera
            float behindCamDistance = Random.Range(10, 15);
            float leftRightDistance = Random.Range(-5, 5);

            Vector3 spawnPosition = mainCam.position - mainCam.forward * behindCamDistance + mainCam.right * leftRightDistance;
            spawnPosition.y = player.transform.position.y;
            GameObject enemy = Instantiate(enemyTypes[index], spawnPosition, Quaternion.identity) as GameObject;
            enemy.transform.parent = parent.transform;

            AIAgent agent = enemy.GetComponent<AIAgent>();
            agent.currentState = State.Attack;

            switch (GameManager.Instance.Difficulty)
            {
                case 0:
                    timer = timeInteval * 1.3f;
                    break;
                case 2:
                    timer = timeInteval * .7f;
                    break;
                default:
                    timer = timeInteval;
                    break;
            }
        }
    }
}
