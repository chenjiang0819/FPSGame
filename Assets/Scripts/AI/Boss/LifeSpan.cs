using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpan : MonoBehaviour
{
    public float lifeSpan = 15f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetDead", lifeSpan);
    }

    void SetDead()
    {
        GetComponent<Rotate>().SetIsDead();
        Destroy(gameObject, 2);
    }
}
