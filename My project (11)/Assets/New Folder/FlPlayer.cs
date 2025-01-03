using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlPlayer : MonoBehaviour
{
    public Rigidbody playerRigidbody;

    void LateUpdate()
    {
        transform.position = playerRigidbody.position;
    }
}