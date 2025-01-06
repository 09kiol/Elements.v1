using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timemanager : MonoBehaviour
{
    public static Timemanager Instance;
    public float TotalPlayTime { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayTime(float time)
    {
        TotalPlayTime = time;
    }
}