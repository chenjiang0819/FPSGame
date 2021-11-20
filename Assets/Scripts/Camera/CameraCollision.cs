using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField] Transform cameraSpring;
    [SerializeField] Transform rawPosition;
    [SerializeField] LayerMask collidingLayerMask;
    [SerializeField] float smooth = .15f;
    [Tooltip("Move the camera forward a little bit to avoid collision.")]
    [SerializeField] float adjustLength = .2f;
    [SerializeField] float springLength = 3f;

    bool isColliding = false;
    RaycastHit hit;

    private void FixedUpdate()
    {
        Vector3 direction = rawPosition.position - cameraSpring.position;
        isColliding = Physics.Raycast(cameraSpring.position, direction, out hit, springLength + 0.1f, collidingLayerMask);
        if (isColliding)
        {
            Vector3 adjustedPosition = hit.point + (cameraSpring.position - hit.point).normalized * adjustLength;
            transform.position = Vector3.Lerp(transform.position, adjustedPosition, smooth);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, rawPosition.position, smooth);
        }
    }
}
