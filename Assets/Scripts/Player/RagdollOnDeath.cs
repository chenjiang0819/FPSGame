using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RagdollOnDeath : MonoBehaviour
{
    public GameObject weapon;
    public Animator SceneTransition;

    public void IsDead()
    {
        Time.timeScale = 0.1f;
        GetComponent<Animator>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<ThirdPersonMovement>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<AnimationStateController>().enabled = false;
        GetComponent<HealthSystem>().enabled = false;
        GetComponent<HealthRecover>().enabled = false;
        GetComponent<RigBuilder>().enabled = false;

        weapon.AddComponent<Rigidbody>();
        weapon.AddComponent<BoxCollider>();
        weapon.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;

        GameObject.Find("HUD").SetActive(false);
        GameObject.Find("PauseMenu").SetActive(false);

        SceneTransition.SetTrigger("FadeOut");

        Invoke("ReloadLevel", 1f);
    }

    void ReloadLevel()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
