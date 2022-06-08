using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    [SerializeField] private Animator autoDoorAnimator;
    private AnimatorStateInfo aniStateInfo;
    private bool doorOpened = false;

    private void OnTriggerStay(Collider other)
    {
        aniStateInfo = autoDoorAnimator.GetCurrentAnimatorStateInfo(0);
        if (aniStateInfo.normalizedTime >= .5f & !doorOpened) {
            autoDoorAnimator.Play("DoorOpen", 0, 0.0f);
            FindObjectOfType<AudioManager>().Play("AutoDoor", 0f);
            doorOpened = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        aniStateInfo = autoDoorAnimator.GetCurrentAnimatorStateInfo(0);
        if (aniStateInfo.normalizedTime >= 1 && doorOpened) {
            autoDoorAnimator.Play("DoorClose", 0, 0.0f);
            FindObjectOfType<AudioManager>().Play("AutoDoor", 0f);
            doorOpened = !doorOpened;
        }
    }
}
