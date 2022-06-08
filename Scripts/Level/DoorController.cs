using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuantumTek.EncryptedSave;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AnimatorStateInfo animatorInfo;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject killer;
    [SerializeField] public bool doorIsOpened = false;
    [SerializeField] private bool playerDoorFront = false;
    [SerializeField] private bool killerDoorFront = false;

    private KeyCode keyInputItem;
    private int doorState;
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
        animatorInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
        playerDoorFront = DoorSide(player.transform.position);
        killerDoorFront = DoorSide(killer.transform.position);
        time += + 1f * Time.deltaTime;
        time2 += + 1f * Time.deltaTime;

        // AI open door
        if (time2 >= AIOpenDoorDelay) {
            if (other.CompareTag("Killer") && animatorInfo.normalizedTime >= 1) {
                if (!doorIsOpened && killerDoorFront && doorState == 0) {
                    AIOpenDoor("DoorOpen", 10, 1, true);
                }
                else if (!doorIsOpened && killerDoorFront && doorState == 2) {
                    AIOpenDoor("DoorOpen", 10, 1, true);
                }
                if (!doorIsOpened && !killerDoorFront && doorState == 0) {
                    AIOpenDoor("DoorOpen2", 10, 3, true);
                }
                else if (!doorIsOpened && !killerDoorFront && doorState == 2) {
                    AIOpenDoor("DoorOpen2", 10, 3, true);
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

        // push
        if (other.CompareTag("Player") && Input.GetKey(keyInputItem) && animatorInfo.normalizedTime >= 1) {
            if (!doorIsOpened && playerDoorFront && doorState == 0) { // outside open door
                DoorStatu("DoorOpen", "OpenDoor", 0f, 1, 10, true);
            } 
            else if (doorIsOpened && playerDoorFront && doorState == 1) { // outside close door
                DoorStatu("DoorClose", "CloseDoor", 0.7f, 0, 0, false);
            }
            else if (doorIsOpened && !playerDoorFront && doorState == 1) { // inside close outside open door
                DoorStatu("DoorClose", "CloseDoor", 0.7f, 2, 0, false);
            }
            else if (!doorIsOpened && !playerDoorFront && doorState == 2) { // inside open door
                DoorStatu("DoorOpen2", "OpenDoor", 0f, 3, 10, true);
            }
            else if (doorIsOpened && !playerDoorFront && doorState == 3) { // inside close door
                DoorStatu("DoorClose2", "CloseDoor", 0.7f, 2, 0, false);
            }
            else if (doorIsOpened && playerDoorFront && doorState == 3) { // outside close inside open door
                DoorStatu("DoorClose2", "CloseDoor", 0.7f, 0, 0, false);
            }
        }
    }

    private bool DoorSide(Vector3 target)
    {
        Vector3 doorRelative = transform.InverseTransformPoint(target);

        if (doorRelative.z < 0)
            return true;
        else
            return false;
    }

    private void DoorStatu(string aniClipName, string audioClipName, float delayTime, int DoorState, int layer, bool doorOpen)
    {// for optimize
        doorAnimator.Play(aniClipName, 0, 0.0f);
        FindObjectOfType<AudioManager>().Play(audioClipName, delayTime);
        doorState = DoorState;
        door.layer = layer;
        doorIsOpened = doorOpen;
    }

    private void AIOpenDoor(string aniClipName, int layer, int DoorState, bool doorOpen)
    {
        doorAnimator.Play(aniClipName, 0, 0.0f);
        door.layer = layer;
        doorState = DoorState;
        doorIsOpened = doorOpen;
    }
}
