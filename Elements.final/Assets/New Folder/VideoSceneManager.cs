using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSceneManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SkipToGameScene();
        }
    }
    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("GAMEPLAY");
    }
    private void SkipToGameScene()
    {
        SceneManager.LoadScene("GAMEPLAY");
    }
}