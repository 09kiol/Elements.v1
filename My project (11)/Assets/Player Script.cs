using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 100f;

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.Self);

        float horizontalInput = Input.GetAxis("RightStickHorizontal");

        transform.Rotate(0, moveHorizontal * rotationSpeed * Time.deltaTime, 0, Space.Self);
        Debug.Log("RightStickHorizontal input: " + moveHorizontal);
    }
}
