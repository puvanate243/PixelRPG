using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField]
    private GameObject Camera;
    [SerializeField]
    private float parallaxSpeed;

    private float startPosition;

    void Start()
    {
        startPosition = transform.position.x;
    }

    void FixedUpdate()
    {
        float distance = (Camera.transform.position.x * parallaxSpeed);
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

    }
}
