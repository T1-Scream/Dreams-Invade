using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonDoor : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;

    private bool doorIsOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!doorIsOpen && other.CompareTag("Player")) {
            doorAnimator.Play("DoorOpen", 0, 0.0f);
            FindObjectOfType<AudioManager>().Play("AutoDoor", 0.2f);
            doorIsOpen = true;
        }
    }
}
