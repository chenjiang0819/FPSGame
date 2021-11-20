using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject background;
    public GameObject mainPanel;
    public GameObject optionPanel;
    public GameObject controllerPanel;
    public GameObject unsavedPanel;
    public GameObject quitPanel;

    public GameObject HUD;

    bool gamePaused = false;
    float timeScale;

    private void Awake()
    {
        timeScale = Time.timeScale;
    }

    private void Start()
    {
        optionPanel.SetActive(false);
        controllerPanel.SetActive(false);
        quitPanel.SetActive(false);
        background.SetActive(false);
        mainPanel.SetActive(false);
        unsavedPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                PauseGame();
            }
            else if (mainPanel.activeSelf)
            {
                UnpauseGame();
            }
            else
            {
                BackToMainPanel();
            }
        }
    }

    public void UnpauseGame()
    {
        background.SetActive(false);
        mainPanel.SetActive(false);
        gamePaused = false;
        HUD.SetActive(true);

        Time.timeScale = timeScale;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void PauseGame()
    {
        gamePaused = true;
        mainPanel.SetActive(true);
        background.SetActive(true);
        HUD.SetActive(false);

        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void BackToMainPanel()
    {
        optionPanel.SetActive(false);
        controllerPanel.SetActive(false);
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);
        unsavedPanel.SetActive(false);
    }

    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
        controllerPanel.SetActive(false);
        quitPanel.SetActive(false);
        mainPanel.SetActive(false);
        unsavedPanel.SetActive(false);
    }

    public void OpenControllerPanel()
    {
        optionPanel.SetActive(false);
        controllerPanel.SetActive(true);
        quitPanel.SetActive(false);
        mainPanel.SetActive(false);
        unsavedPanel.SetActive(false);
    }

    public void OpenQuitPanel()
    {
        optionPanel.SetActive(false);
        controllerPanel.SetActive(false);
        quitPanel.SetActive(true);
        mainPanel.SetActive(false);
        unsavedPanel.SetActive(false);
    }

    public void OpenUnsavedPanel()
    {
        optionPanel.SetActive(false);
        controllerPanel.SetActive(false);
        unsavedPanel.SetActive(true);
        quitPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = timeScale;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }
}
