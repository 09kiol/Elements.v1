using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public string targetSceneName;

    void Update()
    {
        // 检测 Xbox 手柄 A 键（JoystickButton0）
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            // 切换到目标场景
            SceneManager.LoadScene(targetSceneName);
        }
    }
}