using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionLantern : MonoBehaviour
{
    public bool clear = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!clear)
            clear = true;
    }
}
