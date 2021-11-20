using UnityEngine;
using TMPro;
using System;

public class Tutorial : MonoBehaviour
{
    public string lookAroundText;
    public string movementText;
    public string jumpText;
    public string sprintText;
    public string aimText;
    public string shootText;
    public string finishedText;

    public TMP_Text title;
    public TMP_Text content;
    public Mission mission;
    public GameObject mark;

    bool lookAroundTaskFinished = false;
    bool movementTaskFinished = false;
    bool jumpTaskFinished = false;
    bool sprintTaskFinished = false;
    bool aimTaskFinished = false;
    bool shootTaskFinished = false;

    float mouseMovedAmount = 0f;

    void Awake()
    {
        if (this.enabled)
        {
            title.SetText("Tutorial");
            mission.enabled = false;
            mark.SetActive(false);
        }
    }

    void Update()
    {
        if (!lookAroundTaskFinished)
        {
            if (!MouseMoved()) return;
        }

        if (!movementTaskFinished)
        {
            if (!PlayerMoved()) return;
        }

        if (!jumpTaskFinished)
        {
            if (!PlayerJumped()) return;
        }

        if (!sprintTaskFinished)
        {
            if (!PlayerSprinted()) return;
        }

        if (!aimTaskFinished)
        {
            if (!PlayerAimed()) return;
        }

        if (!shootTaskFinished)
        {
            if (!Playershot()) return;
        }

        content.SetText(finishedText);
        Invoke("StartMission", 3);
    }

    private bool PlayerSprinted()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprintTaskFinished = true;
            return true;
        }

        content.SetText(sprintText);
        return false;
    }

    private void StartMission()
    {
        mission.enabled = true;
        mark.SetActive(true);
        Destroy(this);
    }

    private bool Playershot()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            shootTaskFinished = true;
            return true;
        }

        content.SetText(shootText);
        return false;
    }

    private bool PlayerAimed()
    {
        if (Input.GetMouseButton(1))
        {
            aimTaskFinished = true;
            return true;
        }

        content.SetText(aimText);
        return false;
    }

    private bool PlayerJumped()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpTaskFinished = true;
            return true;
        }

        content.SetText(jumpText);
        return false;
    }

    private bool PlayerMoved()
    {
        if (Input.GetKeyDown(KeyCode.W) ||
                        Input.GetKeyDown(KeyCode.A) ||
                        Input.GetKeyDown(KeyCode.S) ||
                        Input.GetKeyDown(KeyCode.D))
        {
            movementTaskFinished = true;
            return true;
        }

        content.SetText(movementText);
        return false;
    }

    private bool MouseMoved()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            mouseMovedAmount += Mathf.Abs(Input.GetAxis("Mouse X")) * Time.deltaTime;
            mouseMovedAmount += Mathf.Abs(Input.GetAxis("Mouse Y")) * Time.deltaTime;

            if (mouseMovedAmount > 1f)
            {
                lookAroundTaskFinished = true;
                return true;
            }
        }

        content.SetText(lookAroundText);
        return false;
    }
}
