using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public string targetSceneName;

    void Update()
    {
        // ��� Xbox �ֱ� A ����JoystickButton0��
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            // �л���Ŀ�곡��
            SceneManager.LoadScene(targetSceneName);
        }
    }
}