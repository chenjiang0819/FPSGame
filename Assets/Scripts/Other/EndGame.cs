using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public Animator sceneTransition;
    public GameObject HUD;
    public GameObject PauseMenu;
    public GameObject EndScreen;
    public GameObject enemies;

    // Start is called before the first frame update
    void Start()
    {
        sceneTransition.SetTrigger("FadeOut");
        HUD.SetActive(false);
        PauseMenu.SetActive(false);

        SceneManager.Instance.player.GetComponent<HealthSystem>().gameEnded = true;
        SceneManager.Instance.player.GetComponent<PlayerController>().Disabled = true;

        AudioManager.Instance.FadeOutAllAndPlay("Victory", .5f);

        Invoke("DisableEnemies", .51f);
        Invoke("DisplayEndScreen", 3f);
        Invoke("BackToMainMenu", 30f);
    }

    private void DisableEnemies()
    {
        enemies.SetActive(false);
    }

    private void DisplayEndScreen()
    {
        EndScreen.SetActive(true);
    }

    private void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
