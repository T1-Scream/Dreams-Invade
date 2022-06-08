using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    [SerializeField] private GameObject[] canPushObj;

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < canPushObj.Length; i++) {
            Rigidbody rb = canPushObj[i].GetComponent<Rigidbody>();
            Destroy(rb);
        }

        FindObjectOfType<PlayerMovement>().canJump = true;
    }
}
