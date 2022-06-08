using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgyptManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private PrisonClear[] clearObj;
    [SerializeField] private Animator missionDoorAnimator;

    private PlayerMovement movement;
    private bool doorIsOpened = false;
    private bool released;

    private void Awake()
    {
        movement = player.GetComponent<PlayerMovement>();
        movement.Speed = 2f;
        movement.canJump = true;
        movement.jumpForce = 3.5f;
        released = false;
    }

    private bool AllItemInputComplete()
    {
        for (int i = 0; i < clearObj.Length; i++) {
            if (clearObj[i].clear == false)
                return false;
        }

        return true;
    }

    private void Update()
    {
        if (AllItemInputComplete() && !doorIsOpened) {
            FindObjectOfType<AudioManager>().Play("AutoDoor", 0f);
            missionDoorAnimator.Play("DoorOpen", 0, 0.0f);
            doorIsOpened = true;
        }

        if (released) return;

        if (!released) {
            FindObjectOfType<PlayerCameraEvent>().SwitchGameTips("Egypt");
            released = true;
        }
    }
}
