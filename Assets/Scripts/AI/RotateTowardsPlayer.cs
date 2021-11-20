using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    public float smooth = 100f;
    GameObject player;

    Vector3 direction;
    Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        player = SceneManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        direction = transform.position - player.transform.position + Vector3.up;
        targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smooth * Time.deltaTime);
    }
}
