using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public Text timerText;
    public float timer = 60.0f;
    bool isTimerRunning = true; // 타이머가 실행 중인지



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
            string timeString = string.Format("{0:00}", seconds);
            timerText.text = "Time: " + timeString;
        }
        else timerText.text = " ";
    }
    

    public void StopTimer()
    {
        // 타이머를 정지합니다.
        isTimerRunning = false;
    }
}
