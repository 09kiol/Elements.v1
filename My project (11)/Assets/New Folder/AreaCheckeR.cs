using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AreaCheckeR : MonoBehaviour
{
    public string enemyTag = "Enemy";        
    public float checkInterval = 2f;         
    public string sceneToLoad = "GOODEND";   
    public float startDelay = 3f;            
    public UiTimer uiTimer;                 

    private bool sceneSwitching = false;    

    private void Start()
    {
        InvokeRepeating(nameof(CheckAllEnemies), startDelay, checkInterval);
        Debug.Log($"将在 {startDelay} 秒后开始检测敌人...");
    }

    private void CheckAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        if (enemies.Length == 0 && !sceneSwitching)
        {
            Debug.Log("场景中没有敌人！3秒后切换场景...");
            sceneSwitching = true;

            if (uiTimer != null)
            {
                float elapsedTime = uiTimer.GetTimeElapsed();
                Timemanager.Instance.SavePlayTime(elapsedTime);
                Debug.Log($"游玩时间已记录：{elapsedTime:F2} 秒");
            }
            else
            {
                Debug.LogWarning("未找到计时器对象！");
            }

            StartCoroutine(LoadNextSceneAfterDelay(3f));
        }
        else
        {
            Debug.Log($"剩余敌人数量：{enemies.Length}");
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("切换场景中...");
        SceneManager.LoadScene(sceneToLoad);
    }
}
