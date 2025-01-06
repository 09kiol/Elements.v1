using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public PlayerController playerController;

    private void Update()
    {
        if (playerController != null && !playerController.enabled)
        {
            playerController.enabled = true;
        }
    }
}
