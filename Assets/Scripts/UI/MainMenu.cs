using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionPanel;
    public GameObject controllerPanel;
    public GameObject creditPanel;
    public GameObject quitPanel;

    private void Start()
    {
        optionPanel.SetActive(false);
        controllerPanel.SetActive(false);
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);
        creditPanel.SetActive(false);
    }

    public void BackToMainPanel()
    {
        optionPanel.SetActive(false);
        controllerPanel.SetActive(false);
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);
        creditPanel.SetActive(false);
    }

    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
        controllerPanel.SetActive(false);
        quitPanel.SetActive(false);
        mainPanel.SetActive(false);
        creditPanel.SetActive(false);
    }

    public void OpenControllerPanel()
    {
        optionPanel.SetActive(false);
        controllerPanel.SetActive(true);
        quitPanel.SetActive(false);
        mainPanel.SetActive(false);
        creditPanel.SetActive(false);
    }

    public void OpenQuitPanel()
    {
        optionPanel.SetActive(false);
        controllerPanel.SetActive(false);
        quitPanel.SetActive(true);
        mainPanel.SetActive(false);
        creditPanel.SetActive(false);
    }

    public void OpenCreditPanel()
    {
        optionPanel.SetActive(false);
        controllerPanel.SetActive(false);
        creditPanel.SetActive(true);
        quitPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartNewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }
}
