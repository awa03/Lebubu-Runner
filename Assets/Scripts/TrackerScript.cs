using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackerScript : MonoBehaviour { 

    public TextMeshProUGUI timeText;
    float startTime;

    void Start()
    {
        startTime = Time.time;
    }
    public void UpdateTimeText()
    { 
        float elapsedTime = Time.time - startTime;
        timeText.text = elapsedTime.ToString("F2") + " seconds";
    }
}