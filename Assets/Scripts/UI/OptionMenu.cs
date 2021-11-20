using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class OptionMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown difficultyDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Slider mouseSensitivitySlider;
    public Slider masterVolumeSlider;
    public Slider bGMVolumeSlider;
    public Slider sFXVolumeSlider;

    Resolution[] resolutions;

    int difficulty;
    Resolution resolution;
    bool isFullscreen;
    int qualityIndex;
    float mouseSensitivity;
    float masterVolume;
    float bGMVolume;
    float sFXVolume;

    // Start is called before the first frame update
    void Start()
    {
        PopulateResolutionDropdown();

        qualityIndex = QualitySettings.GetQualityLevel();
        qualityDropdown.value = qualityIndex;
        qualityDropdown.RefreshShownValue();

        difficulty = GameManager.Instance.Difficulty;
        difficultyDropdown.value = difficulty;
        difficultyDropdown.RefreshShownValue();

        isFullscreen = Screen.fullScreen;

        mouseSensitivity = GameManager.Instance.MouseSensitivity;
        mouseSensitivitySlider.value = mouseSensitivity;

        audioMixer.GetFloat("MasterVolume", out masterVolume);
        audioMixer.GetFloat("BGMVolume", out bGMVolume);
        audioMixer.GetFloat("SFXVolume", out sFXVolume);
        masterVolumeSlider.value = masterVolume;
        bGMVolumeSlider.value = bGMVolume;
        sFXVolumeSlider.value = sFXVolume;
    }

    public void SetResolution(int resolutionIndex)
    {
        resolution = resolutions[resolutionIndex];
    }

    private void PopulateResolutionDropdown()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetQuality(int qualityIndex)
    {
        this.qualityIndex = qualityIndex;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        this.isFullscreen = isFullscreen;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }

    public void SetBGMVolume(float volume)
    {
        bGMVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sFXVolume = volume;
    }

    public void SetMouseSensitivity(float mouseSensitivity)
    {
        this.mouseSensitivity = mouseSensitivity;
    }

    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
    }

    public void ApplySettings()
    {
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        QualitySettings.SetQualityLevel(qualityIndex);
        Screen.fullScreen = isFullscreen;
        GameManager.Instance.Difficulty = difficulty;
        GameManager.Instance.MouseSensitivity = mouseSensitivity;
        audioMixer.SetFloat("MasterVolume", masterVolume);
        audioMixer.SetFloat("BGMVolume", bGMVolume);
        audioMixer.SetFloat("SFXVolume", sFXVolume);
    }
}
