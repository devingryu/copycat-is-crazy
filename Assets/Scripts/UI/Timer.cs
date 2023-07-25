using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float totalTime = 120.0f; // Ÿ�̸Ӱ� ���ۺ��� ��������� �� �ð� (��)
    private float currentTime; // ���� �ð� (��)
    private TMP_Text timerText; // TMP_Text ������Ʈ

    void Start()
    {
        timerText = GetComponent<TMP_Text>(); // TMP_Text ������Ʈ ��������
        currentTime = totalTime; // �ʱ�ȭ
        UpdateTimerText(); // TMP_Text ������Ʈ
    }

    void Update()
    {
        // �ð��� ���ҽ�Ű�� UI Text ������Ʈ
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            // Ÿ�̸Ӱ� ����Ǿ��� �� ó��
            if (currentTime <= 0)
            {
                currentTime = 0;
                GameManager.Instance.EndGame();
                // Ÿ�̸� ���� �� �߰����� �۾� ���� (��: ���� ����, �޽��� ǥ�� ��)
                // �� �κ��� ���ϴ� ��� �������ּ���.
            }

            UpdateTimerText(); // UI Text ������Ʈ
        }
    }

    void UpdateTimerText()
    {
        // ���� �ð��� �а� �ʷ� ��ȯ�Ͽ� ǥ��
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // �ð��� 00:00 �������� ����
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        // TMP_Text�� Ÿ�̸� �ؽ�Ʈ ǥ��
        timerText.text = "Timer: " + timeString;
    }
}