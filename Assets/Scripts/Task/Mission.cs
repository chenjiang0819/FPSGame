using UnityEngine;
using TMPro;

public class Mission : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text content;

    public string finishedText;
    public string[] texts;
    public GameObject[] targets;

    int stage = 0;

    // Start is called before the first frame update
    void Start()
    {
        title.SetText("Mission");
        if (stage == targets.Length)
        {
            content.SetText(finishedText);
            return;
        }
        content.SetText(texts[stage]);
        targets[stage].SetActive(true);
    }

    public void UpdateMission()
    {
        targets[stage].SetActive(false);
        stage++;

        if (stage >= targets.Length)
        {
            return;
        }

        content.SetText(texts[stage]);
    }

    public GameObject CurrentTarget()
    {
        if (stage >= targets.Length) return null;
        return targets[stage];
    }
}
