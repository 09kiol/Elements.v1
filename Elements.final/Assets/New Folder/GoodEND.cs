using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodEND : MonoBehaviour
{
    public UiTimer uiTimer; 
    public string targetScene = "SummaryScene";  

    public void SwitchScene()
    {
        if (uiTimer != null && Timemanager.Instance != null)
        {
            float playTime = uiTimer.GetTimeElapsed();
            Timemanager.Instance.SavePlayTime(playTime);
            Debug.Log($"保存时间成功: {playTime:F2} 秒");
        }
        else
        {
            Debug.LogWarning("未找到 UiTimer 或 Timemanager 实例！");
        }

        SceneManager.LoadScene(targetScene);
    }
}