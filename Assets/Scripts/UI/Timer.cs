using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    public float totalTime = 120.0f; // 타이머가 시작부터 종료까지의 총 시간 (초)
    private float currentTime; // 현재 시간 (초)
    private TMP_Text timerText; // TMP_Text 컴포넌트
    bool isSpurt = false;

    void Start()
    {
        timerText = GetComponent<TMP_Text>(); // TMP_Text 컴포넌트 가져오기
        currentTime = totalTime; // 초기화
        UpdateTimerText(); // TMP_Text 업데이트
    }

    void Update()
    {
        // 시간을 감소시키고 UI Text 업데이트
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.T))
            {
                currentTime -= 10;
            }

            // 타이머가 종료되었을 때 처리
            if (currentTime <= 0)
            {
                currentTime = 0;
                GameManager.Instance.EndGame();
                // 타이머 종료 시 추가적인 작업 수행 (예: 게임 종료, 메시지 표시 등)
                // 이 부분을 원하는 대로 수정해주세요.
            }

            UpdateTimerText(); // UI Text 업데이트

            if (!isSpurt && currentTime <= 30)
            {
                isSpurt = true;
                AudioSource source = Camera.main.GetComponent<AudioSource>();
                source.Stop();
                source.clip = clip;
                source.Play();
                Debug.Log(source.isPlaying);
            }

        }
    }

    void UpdateTimerText()
    {
        // 남은 시간을 분과 초로 변환하여 표시
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // 시간을 00:00 형식으로 포맷
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        // TMP_Text에 타이머 텍스트 표시
        timerText.text = "Timer: " + timeString;
    }
}
