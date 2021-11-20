using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject muzzleCone;

    // Start is called before the first frame update
    void Start()
    {
        var mat = muzzleCone.GetComponent<ParticleSystemRenderer>().material;
        int xScale = Random.Range(2, 6);
        mat.mainTextureScale = new Vector2(xScale, 1);
    }
}
