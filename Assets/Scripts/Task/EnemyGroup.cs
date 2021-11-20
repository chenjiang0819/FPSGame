using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGroup : MonoBehaviour
{
    GameObject player;
    Mission mission;

    // Start is called before the first frame update
    void Start()
    {
        player = SceneManager.Instance.player;
        mission = SceneManager.Instance.mission;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            mission.UpdateMission();
            Destroy(gameObject, 5f);
        }
    }
}
