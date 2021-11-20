using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Difficulty { get; set; }
    public float MouseSensitivity { get; set; }
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        Difficulty = 1;
        MouseSensitivity = 1f;

        DontDestroyOnLoad(this.gameObject);
    }
}
