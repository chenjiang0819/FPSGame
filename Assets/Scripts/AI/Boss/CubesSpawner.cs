using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesSpawner : MonoBehaviour
{
    public GameObject cube;
    public Transform pivot;
    public Transform[] pos = new Transform[6];

    public float rotateSpeed = 15f;
    public float flySpeed = 20f;
    public float spawnRate = .4f;

    public float minDamage = 10f;
    public float maxDamage = 20f;

    int numCubes = 6;

    GameObject copy;
    GameObject player;

    bool attacking = false;

    private void Start()
    {
        player = SceneManager.Instance.player;
    }

    void Update()
    {
        Rotate();
    }

    public void ApplyDamage()
    {
        var health = player.GetComponent<HealthSystem>();
        health.TakeDamage(minDamage, maxDamage, 0, false);
    }

    public void StartAttack()
    {
        if (attacking) return;

        StartCoroutine("SpawnCubes");
        attacking = true;
    }

    public void StopAttack()
    {
        if (!attacking) return;

        StopCoroutine("SpawnCubes");
        attacking = false;
    }

    IEnumerator SpawnCubes()
    {
        for (int i = 0; i < numCubes; i++)
        {
            copy = GameObject.Instantiate(cube, transform.position, transform.rotation);
            copy.transform.localScale = Vector3.one * 30;
            copy.transform.parent = transform.parent;
            var script = copy.GetComponent<FollowTarget>();
            script.target = pos[i];
            script.speed = flySpeed;
            yield return new WaitForSeconds(1f / spawnRate);
        }
    }

    private void Rotate()
    {
        pivot.Rotate(pivot.up, rotateSpeed * Time.deltaTime);
    }
}
