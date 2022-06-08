using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonClear : MonoBehaviour
{
    [SerializeField] private string objTag;
    public bool clear = false;

    private float time = 0f;
    private float delayTime = 3f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(objTag)) {
            time += + 1f * Time.deltaTime;
            if (time >= delayTime)
                clear = true;
        }
    }
}
