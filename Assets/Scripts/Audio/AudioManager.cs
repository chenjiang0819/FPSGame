using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup mixerGroup;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    public void FadeOutAllAndPlay(string name, float fadeTime)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                StartCoroutine(FadeIn(s.name, fadeTime));
            }
            else
            {
                if (s.source.isPlaying)
                    StartCoroutine(FadeOut(s.name, fadeTime));
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound (" + name + ") was not found.");
        }

        s.source.Play();
    }

    public IEnumerator FadeOut(string name, float fadeTime)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        AudioSource audioSource = s.source;
        float startVolume = s.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public IEnumerator FadeIn(string name, float fadeTime)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        AudioSource audioSource = s.source;
        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.time = s.startTime;
        audioSource.Play();

        while (audioSource.volume < s.volume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.volume = s.volume;
    }
}
