using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform playerTransform;
    public float offset;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        Vector3 temp = transform.position;

        temp.x = playerTransform.position.x + offset;
        temp.y = playerTransform.position.y;

        transform.position = temp;
    }
}
