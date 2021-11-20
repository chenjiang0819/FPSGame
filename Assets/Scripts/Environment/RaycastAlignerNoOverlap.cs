using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAlignerNoOverlap : MonoBehaviour
{
    public GameObject[] itemsToPickFrom;
    public float raycastDistance = 1000f;
    public float overlapBoxSize = 3f;
    public LayerMask spwanedObjectLayer;
    Collider[] collidersInsideOverlapBox = new Collider[1];
    public float maxScale = 3f;
    public float minScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        PositionRaycast();
    }

    void PositionRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            Quaternion spawnRotation = Quaternion.AngleAxis(Random.Range(0, 360), hit.normal);

            Vector3 overlapBoxScale = new Vector3(overlapBoxSize, overlapBoxSize, overlapBoxSize);
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapBoxScale, collidersInsideOverlapBox, spawnRotation, spwanedObjectLayer);

            if (numberOfCollidersFound == 0)
            {
                Pick(hit.point, spawnRotation);
            }
        }
    }

    GameObject InstantiateRandomScale(GameObject source)
    {
        source.transform.localScale *= Random.Range(minScale, maxScale);
        return source;
    }

    void Pick(Vector3 positionToSpawn, Quaternion spawnRotation)
    {
        int randomIndex = Random.Range(0, itemsToPickFrom.Length);
        GameObject item = itemsToPickFrom[randomIndex];
        GameObject clone = InstantiateRandomScale(Instantiate(item, positionToSpawn, spawnRotation));
        clone.transform.parent = transform;
    }
}
