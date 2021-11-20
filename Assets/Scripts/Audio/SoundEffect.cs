using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioClip[] clipsToChoseFrom;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (clipsToChoseFrom.Length <= 0)
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clipsToChoseFrom[Random.Range(0, clipsToChoseFrom.Length)];
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
