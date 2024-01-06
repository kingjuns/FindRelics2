using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public Text timerText;
    public float timer = 60.0f;
    bool isTimerRunning = true; // 타이머가 실행 중인지

    [HideInInspector]
    public int currentSeconds;

    private void Start()
    {
        currentSeconds = 0;
    }

    void Update()
    {
        if (isTimerRunning && timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();
        }
        else if (isTimerRunning && timer <= 0)
        {
            timer = 0;
            isTimerRunning = false;
            Instantiate(GameManager.Instance.UI_Over_Prefab);
        }
    }

    void UpdateTimerText()
    {
        if (timer > 0)
        {
            // 시간을 분과 초로 변환하여 Text에 표시합니다.
            int seconds = Mathf.CeilToInt(timer);
            currentSeconds = seconds;
            string timeString = string.Format("{0:00}", seconds);
            timerText.text = timeString;
        }
        else
        {
            timerText.text = " ";
        }
    }


    public void StopTimer()
    {
        // 타이머를 정지합니다.
        isTimerRunning = false;
        
    }

    public int ResultPercent()
    {
        int result = -1;

        if (currentSeconds >= 0)
            result = 10;
        if (currentSeconds >= 30)
            result = 7;
        if (currentSeconds >= 35)
            result = 5;
        if (currentSeconds >= 40)
            result = 1;

        return result;
    }
}
