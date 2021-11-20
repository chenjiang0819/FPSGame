using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Animator sceneTransitionAnimator;
    GameObject player;

    private void Start()
    {
        player = SceneManager.Instance.player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            sceneTransitionAnimator.SetTrigger("FadeOut");
            Invoke("LoadBossScene", .7f);
        }
    }

    private void LoadBossScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2, UnityEngine.SceneManagement.LoadSceneMode.Single);
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
    }
}
