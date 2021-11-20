using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    public GameObject postProcess;
    public GameObject mainCam;
    public GameObject cameraSpring;
    public HeadshotIndicator headshotIndicator;
    public Mission mission;
    public static SceneManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        // Cache references to all desired variables
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Boss");
        postProcess = GameObject.Find("PostProcess");
        mainCam = GameObject.FindGameObjectWithTag("MainCam");
        cameraSpring = GameObject.FindGameObjectWithTag("CameraSpring");
        headshotIndicator = GameObject.Find("HeadshotIndicator").GetComponent<HeadshotIndicator>();
        mission = GameObject.Find("MissionPanel").GetComponent<Mission>();
    }
}
