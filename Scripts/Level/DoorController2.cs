using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuantumTek.EncryptedSave;

public class DoorController2 : MonoBehaviour
{
    [SerializeField] private Animator LeftDoorAnimator;
    [SerializeField] private Animator RightDoorAnimator;
    [SerializeField] private AnimatorStateInfo LeftDoorAnimatorInfo;
    [SerializeField] private AnimatorStateInfo RightDoorAnimatorInfo;
    [SerializeField] private GameObject[] door;
    [SerializeField] public bool doorIsOpened = false;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject killer;

    private int doorState;
    private KeyCode keyInputItem;
    private bool playerDoorFront = false;
    private bool killerDoorFront = false;
    private float time;
    private float time2;
    private float timeDelay = 1.7f;
    private float AIOpenDoorDelay = 3f;

    private void Awake()
    {
        keyInputItem = (KeyCode)System.Enum.Parse(typeof(KeyCode), ES_Save.Load<string>("masterKeyItem"));
        doorState = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        LeftDoorAnimatorInfo = LeftDoorAnimator.GetCurrentAnimatorStateInfo(0);
        RightDoorAnimatorInfo = RightDoorAnimator.GetCurrentAnimatorStateInfo(0);
        playerDoorFront = TargetInDoorDirection(player.transform.position);
        killerDoorFront = TargetInDoorDirection(killer.transform.position);
        time += + 1f * Time.deltaTime;
        time2 += + 1f * Time.deltaTime;

        if (time2 >= AIOpenDoorDelay) {
            if (other.CompareTag("Killer") && LeftDoorAnimatorInfo.normalizedTime >= 1 && RightDoorAnimatorInfo.normalizedTime >= 1) {
                if (!doorIsOpened && killerDoorFront && doorState == 0) {
                    AIOpenDoor("DoorOpen_Left", "DoorOpen_Right", 10, 1, true);
                }
                if (!doorIsOpened && killerDoorFront && doorState == 2) {
                    AIOpenDoor("DoorOpen_Left", "DoorOpen_Right", 10, 1, true);
                }
                else if (!doorIsOpened && !killerDoorFront && doorState == 0) {
                    AIOpenDoor("DoorOpen_Left2", "DoorOpen_Right2", 10, 3, true);
                }
                else if (!doorIsOpened && !killerDoorFront && doorState == 2) {
                    AIOpenDoor("DoorOpen_Left2", "DoorOpen_Right2", 10, 3, true);
                }
            }
            time2 = 0f;
        }

        if (other.CompareTag("Player") && playerDoorFront) {
            if (!doorIsOpened) {
                ToolTipsManager.instance.ShowToolTip(Lean.Localization.LeanLocalization.GetTranslationText("OpenDoor"));
                if (time >= timeDelay) {
                    time = 0f;
                    if (Input.GetKey(keyInputItem))
                        doorState = 0;
                }
            }
            else {
                ToolTipsManager.instance.ShowToolTip(Lean.Localization.LeanLocalization.GetTranslationText("CloseDoor"));
                if (time >= timeDelay) {
                    time = 0f;
                    if (Input.GetKey(keyInputItem))
                        doorState = 1;
                }
            }
        }
        else if (other.CompareTag("Player") && !playerDoorFront) {
            if (!doorIsOpened) {
                ToolTipsManager.instance.ShowToolTip(Lean.Localization.LeanLocalization.GetTranslationText("OpenDoor"));
                if (time >= timeDelay) {
                    time = 0f;
                    if (Input.GetKey(keyInputItem))
                        doorState = 2;
                }
            }
            else {
                ToolTipsManager.instance.ShowToolTip(Lean.Localization.LeanLocalization.GetTranslationText("CloseDoor"));
                if (time >= timeDelay) {
                    time = 0f;
                    if (Input.GetKey(keyInputItem))
                        doorState = 3;
                }
            }
        }

        if (other.CompareTag("Player") && Input.GetKey(keyInputItem) && LeftDoorAnimatorInfo.normalizedTime >= 1 && RightDoorAnimatorInfo.normalizedTime >= 1) {
            if (!doorIsOpened && playerDoorFront && doorState == 0) { // outside open door
                DoorStatu("DoorOpen_Left", "DoorOpen_Right", "OpenDoor", 0f, 1, true);
            }
            else if (doorIsOpened && playerDoorFront && doorState == 1) { // outside close door
                DoorStatu("DoorClose_Left", "DoorClose_Right", "CloseDoor", 0.7f, 0, false);
            }
            else if (doorIsOpened && !playerDoorFront && doorState == 1) { // inside close outside open door
                DoorStatu("DoorClose_Left", "DoorClose_Right", "CloseDoor", 0.7f, 2, false);
            }
            else if (!doorIsOpened && !playerDoorFront && doorState == 2) { // inside open door
                DoorStatu("DoorOpen_Left2", "DoorOpen_Right2", "OpenDoor", 0f, 3, true);
            }
            else if (doorIsOpened && !playerDoorFront && doorState == 3) { // inside close door
                DoorStatu("DoorClose_Left2", "DoorClose_Right2", "CloseDoor", 0.7f, 2, false);
            }
            else if (doorIsOpened && playerDoorFront && doorState == 3) { // outside close inside open door
                DoorStatu("DoorClose_Left2", "DoorClose_Right2", "CloseDoor", 0.7f, 0, false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ToolTipsManager.instance.HideToolTip();
    }

    private bool TargetInDoorDirection(Vector3 target)
    {
        Vector3 doorRelative = transform.InverseTransformPoint(target);

        if (doorRelative.z < 0) {
            return true;
        }
        else {
            return false;
        }
    }

    private void DoorStatu(string aniClipName, string aniClipName2, string audioClipName, float delayTime, int DoorState, bool doorOpen)
    {// for optimize
        LeftDoorAnimator.Play(aniClipName, 0, 0.0f);
        RightDoorAnimator.Play(aniClipName2, 0, 0.0f);
        FindObjectOfType<AudioManager>().Play(audioClipName, delayTime);
        doorState = DoorState;
        doorIsOpened = doorOpen;
    }

    private void AIOpenDoor(string aniClipName, string aniClipName2, int layer, int DoorState, bool doorOpen)
    {
        LeftDoorAnimator.Play(aniClipName, 0, 0.0f);
        RightDoorAnimator.Play(aniClipName2, 0, 0.0f);
        door[0].layer = layer;
        door[1].layer = layer;
        doorState = DoorState;
        doorIsOpened = doorOpen;
    }
}
