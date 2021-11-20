using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFrameRate : MonoBehaviour
{
    public bool limited = false;
    public int limitedFrameRate = 60;

    // Start is called before the first frame update
    void Start()
    {
        if (limited)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = limitedFrameRate;
        }
    }
}
