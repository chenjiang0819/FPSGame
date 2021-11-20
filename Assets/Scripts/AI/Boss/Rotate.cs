using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float baseCubeSpeed = 10f;
    public float lowCubeSpeed = 5f;
    public float highCubeSpeed = 5f;
    public float deadSpeed = 2f;
    public Transform baseCube;
    public Transform lowCube;
    public Transform highCube;

    public float randomMin = 50f;
    public float randomMax = 60f;

    public float scale = 0.1f;
    public bool bossStyle = true;

    Quaternion baseRotation;
    Quaternion lowRotation;
    Quaternion highRotation;

    bool isDead;

    private void Start()
    {
        baseRotation = baseCube.rotation;
        lowRotation = lowCube.rotation;
        highRotation = highCube.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
            KeepRotating();
        else
        {
            baseCube.rotation = Quaternion.Slerp(baseCube.rotation, baseRotation, Time.deltaTime * deadSpeed);
            lowCube.rotation = Quaternion.Slerp(lowCube.rotation, lowRotation, Time.deltaTime * deadSpeed);
            highCube.rotation = Quaternion.Slerp(highCube.rotation, highRotation, Time.deltaTime * deadSpeed);
        }

    }

    public void SetIsDead()
    {
        isDead = true;
    }

    private void KeepRotating()
    {
        if (bossStyle)
        {
            baseCube.Rotate(baseCube.up, Random.Range(randomMin, randomMax) * scale * Time.deltaTime);
            baseCube.Rotate(baseCube.forward, Random.Range(randomMin, randomMax) * scale * Time.deltaTime);
            baseCube.Rotate(baseCube.right, Random.Range(randomMin, randomMax) * scale * Time.deltaTime);

            lowCube.Rotate(lowCube.up, Random.Range(randomMin, randomMax) * scale * Time.deltaTime);
            lowCube.Rotate(lowCube.forward, Random.Range(randomMin, randomMax) * scale * Time.deltaTime);
            lowCube.Rotate(lowCube.right, Random.Range(randomMin, randomMax) * scale * Time.deltaTime);

            highCube.Rotate(highCube.up, Random.Range(randomMin, randomMax) * scale * Time.deltaTime);
            highCube.Rotate(highCube.forward, Random.Range(randomMin, randomMax) * scale * Time.deltaTime);
            highCube.Rotate(highCube.right, Random.Range(randomMin, randomMax) * scale * Time.deltaTime);
        }
        else
        {
            baseCube.Rotate(baseCube.up, baseCubeSpeed * Time.deltaTime);
            baseCube.Rotate(baseCube.forward, baseCubeSpeed * Time.deltaTime);
            baseCube.Rotate(baseCube.right, baseCubeSpeed * Time.deltaTime);

            lowCube.Rotate(lowCube.up, lowCubeSpeed * Time.deltaTime);
            lowCube.Rotate(lowCube.forward, lowCubeSpeed * Time.deltaTime);
            lowCube.Rotate(lowCube.right, lowCubeSpeed * Time.deltaTime);

            highCube.Rotate(highCube.up, -highCubeSpeed * Time.deltaTime);
            highCube.Rotate(highCube.forward, -highCubeSpeed * Time.deltaTime);
            highCube.Rotate(highCube.right, -highCubeSpeed * Time.deltaTime);
        }
    }
}
