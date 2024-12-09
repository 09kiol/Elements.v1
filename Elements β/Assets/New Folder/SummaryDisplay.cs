using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryDisplay : MonoBehaviour
{
    public Text summaryText;  

    private void Start()
    {
        if (Timemanager.Instance == null)
        {
            Debug.LogError("未找到 Timemanager 实例！");
            summaryText.text = "时间加载失败";
            return;
        }

        float totalPlayTime = Timemanager.Instance.TotalPlayTime;
        int minutes = Mathf.FloorToInt(totalPlayTime / 60);
        int seconds = Mathf.FloorToInt(totalPlayTime % 60);

        if (summaryText != null)
        {
            summaryText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogError("未设置 UI 文本对象！");
        }
    }
}