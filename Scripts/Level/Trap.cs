using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private BoxCollider boxCollider;
    private float time;
    private float timeDelay = 2f;

    private void OnTriggerEnter(Collider other)
    {
        boxCollider = transform.parent.GetComponent<BoxCollider>();
        transform.parent.gameObject.tag = "Untagged"; //ensure trap on can't jump
    }

    private void OnTriggerStay(Collider other)
    {
        time += + 1f * Time.deltaTime;

        if (time >= timeDelay) {
            boxCollider.isTrigger = true;
            time = 0f;
        }
    }
}
