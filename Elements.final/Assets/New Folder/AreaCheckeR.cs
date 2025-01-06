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
        Debug.Log($"���� {startDelay} ���ʼ������...");
    }

    private void CheckAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        if (enemies.Length == 0 && !sceneSwitching)
        {
            Debug.Log("������û�е��ˣ�3����л�����...");
            sceneSwitching = true;

            if (uiTimer != null)
            {
                float elapsedTime = uiTimer.GetTimeElapsed();
                Timemanager.Instance.SavePlayTime(elapsedTime);
                Debug.Log($"����ʱ���Ѽ�¼��{elapsedTime:F2} ��");
            }
            else
            {
                Debug.LogWarning("δ�ҵ���ʱ������");
            }

            StartCoroutine(LoadNextSceneAfterDelay(3f));
        }
        else
        {
            Debug.Log($"ʣ�����������{enemies.Length}");
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("�л�������...");
        SceneManager.LoadScene(sceneToLoad);
    }
}
