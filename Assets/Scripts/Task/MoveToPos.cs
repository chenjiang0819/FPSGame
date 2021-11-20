using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPos : MonoBehaviour
{
    GameObject player;
    Mission mission;

    // Start is called before the first frame update
    void Start()
    {
        player = SceneManager.Instance.player;
        mission = SceneManager.Instance.mission;
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            mission.UpdateMission();
        }
    }
}
