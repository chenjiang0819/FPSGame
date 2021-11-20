using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform rawPosition;

    [Header("Properties")]
    [SerializeField] float heightOffset = 1.4f;
    [SerializeField] float springLength = 3f;
    [SerializeField] float sensitivity = 1f;
    [SerializeField] float xMinRotation = -25;
    [SerializeField] float xMaxRotation = 85;

    [Header("Aiming")]
    [SerializeField] Vector3 normalCameraOffset = Vector3.zero;
    [SerializeField] Vector3 aimingCameraOffset = Vector3.zero;
    [SerializeField] float normalFov = 70;
    [SerializeField] float aimingFov = 50;
    [SerializeField] float zoomDuration = .1f;

    GameObject player;
    PlayerController playerController;
    float mouseX = 0f, mouseY = 0f;
    bool isAiming = false;
    Camera mainCam;
    Vector3 cameraOffset = Vector3.zero;
    float timeElapsed = 0f;

    public float Sensitivity { get => sensitivity; set => sensitivity = value; }

    private void Start()
    {
        player = SceneManager.Instance.player;
        mainCam = GetComponentInChildren<Camera>();
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        Sensitivity = GameManager.Instance.MouseSensitivity;

        GetInput();
        Zoom();
        targetPosition.localPosition = Vector3.forward * -1 * springLength;
        rawPosition.localPosition = cameraOffset;
    }

    private void LateUpdate()
    {
        FollowTarget();
        OrbitTarget();
    }

    private void GetInput()
    {
        mouseX = playerController.MouseX;
        mouseY = playerController.MouseY;
        isAiming = playerController.IsAiming;
    }

    private void Zoom()
    {
        if (isAiming && timeElapsed < zoomDuration)
        {
            if (timeElapsed < zoomDuration)
                timeElapsed += Time.deltaTime;
            else
                timeElapsed = zoomDuration;
        }

        if (!isAiming && timeElapsed > 0)
        {
            if (timeElapsed > 0f)
                timeElapsed -= Time.deltaTime;
            else
                timeElapsed = 0f;
        }
        mainCam.fieldOfView = Mathf.Lerp(normalFov, aimingFov, timeElapsed / zoomDuration);
        cameraOffset = Vector3.Lerp(normalCameraOffset, aimingCameraOffset, timeElapsed / zoomDuration);
    }

    private void OrbitTarget()
    {
        float xRotation = -mouseY * 90 * Sensitivity * Time.deltaTime + transform.eulerAngles.x;
        float yRotation = mouseX * 90 * Sensitivity * Time.deltaTime + transform.eulerAngles.y;

        xRotation = Mathf.Clamp(NormalizeAngle(xRotation), xMinRotation, xMaxRotation);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void FollowTarget()
    {
        transform.position = player.transform.position + heightOffset * Vector3.up;
    }

    private float NormalizeAngle(float a)
    {
        return a > 90f ? a - 360f : a;
    }
}
