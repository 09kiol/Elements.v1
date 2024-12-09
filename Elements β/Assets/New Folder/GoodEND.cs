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
            Debug.Log($"����ʱ��ɹ�: {playTime:F2} ��");
        }
        else
        {
            Debug.LogWarning("δ�ҵ� UiTimer �� Timemanager ʵ����");
        }

        SceneManager.LoadScene(targetScene);
    }
}