using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpBoss : MonoBehaviour
{
    public GameObject prefabToSpawn;  
    public float spawnTime = 30f;     
    private float timeElapsed = 0f;   

    void Update()
    {      
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= spawnTime)
        {
            Instantiate(prefabToSpawn, transform.position, transform.rotation);

            enabled = false;
        }
    }
}