using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseMove : MonoBehaviour
{
    [HideInInspector] public float sensitivity;
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;

    private GameObject player;

    private void Start()
    {
        player = gameObject.transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        sensitivity = FindObjectOfType<GameManager>().sensitivityData * 50f;
    }

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.transform.Rotate(Vector3.up * mouseX);
    }
}
