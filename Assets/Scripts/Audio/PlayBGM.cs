using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    public string clipName;
    public float fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.FadeOutAllAndPlay(clipName, fadeTime);
    }
}
