using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mark : MonoBehaviour
{
    public TMP_Text meter;

    Mission mission;
    Camera mainCam;
    GameObject player;
    Vector3 screenPos;

    float distance;

    // Start is called before the first frame update
    void Start()
    {
        mission = SceneManager.Instance.mission;
        mainCam = SceneManager.Instance.mainCam.GetComponent<Camera>();
        player = SceneManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (mission.CurrentTarget() == null) return;
        screenPos = mainCam.WorldToScreenPoint(mission.CurrentTarget().transform.position);

        screenPos.x = Mathf.Clamp(screenPos.x, 100, Screen.currentResolution.width - 100);
        screenPos.y = Mathf.Clamp(screenPos.y, 100, Screen.currentResolution.height - 200);

        if (screenPos.z >= 0)
        {
            transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
        }

        distance = Vector3.Magnitude(mission.CurrentTarget().transform.position - player.transform.position);
        meter.text = (int)distance + "M";
    }
}
