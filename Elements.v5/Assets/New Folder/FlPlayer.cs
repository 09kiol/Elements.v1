using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlPlayer : MonoBehaviour
{
    public Rigidbody playerRigidbody;

    void FixedUpdate()
    {
        transform.position = playerRigidbody.position;
    }
}