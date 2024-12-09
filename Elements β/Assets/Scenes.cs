using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public string targetSceneName;
    public string alternateSceneName;

    void Update()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                SceneManager.LoadScene(targetSceneName);
            }

            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                SceneManager.LoadScene(alternateSceneName);
            }
        }
    }
}