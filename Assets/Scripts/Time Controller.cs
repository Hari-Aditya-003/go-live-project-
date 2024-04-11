using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public float totalTime = 30f;
    private float currentTime;
    public Text timerText;

    void Start()
    {
        currentTime = totalTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
         
            Debug.Log("Time's up! Game Over!");
            
        }
    }

    void UpdateTimerUI()
    {
        timerText.text = "Time: " + Mathf.Round(currentTime).ToString();
    }
}
