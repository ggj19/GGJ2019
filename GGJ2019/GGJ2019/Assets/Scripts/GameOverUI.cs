using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Text timeText;
    private float currentSecond;
    float fsecond;
    

    private void OnEnable()
    {
        
    }
    // 초 단위로 시간만 넣어주세요
    public void SetGameOverTime(float second)
    {
        fsecond = second;
        StartCoroutine("AnimEnd");
    }

    private void UpdateText(float second) // 초 단위
    {
        timeText.text = string.Format("{0:#00}:{1:00}.{2:00}",
            Mathf.Floor(second / 60),           //minutes
            Mathf.Floor(second) % 60,           // seconds
            Mathf.Floor((second*100)%100));     // miliseconds
        //timeText.text = $"Survive Time {(int)second / 60}:{second % 60}";
    }

    private IEnumerator AnimEnd()
    {
        yield return new WaitForSeconds(0.7f);
        StartCoroutine("UpCount");
        StopCoroutine("AnimEnd");
    }

    private IEnumerator UpCount()
    {
        currentSecond = 0;
        while (currentSecond < fsecond)
        {
            yield return new WaitForSeconds(0.01f);
            currentSecond += fsecond / 100;
            UpdateText(currentSecond);
        }
        //UpdateText(fsecond);
        StopCoroutine("UpCount");
        Time.timeScale = 0f;
    }
}
