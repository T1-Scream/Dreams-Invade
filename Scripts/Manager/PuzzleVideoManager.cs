using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleVideoManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject tipsMessage;
    [SerializeField] private GameObject tipImageParent;

    private PlayerMovement movement;
    private float countDownTimer;
    private bool showTips;
    private bool released;
    private bool activeTrue;
    private bool activeFalse;
    private GameManager gameManager;

    private void Awake()
    {
        movement = player.GetComponent<PlayerMovement>();
        movement.Speed = 1.8f;
        movement.standSpeed = 1.8f;
        movement.jumpForce = 3.1f;
        movement.canCrouch = true;
        movement.canJump = true;
        showTips = false;
        released = false;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (released) return;

        countDownTimer = gameManager.countDownTime;

        if (countDownTimer <= 360f && !activeTrue) {
            tipsMessage.SetActive(true);
            activeTrue = true;
        }

        if (countDownTimer <= 350f && !activeFalse) {
            tipsMessage.SetActive(false);
            activeFalse = true;
        }

        if (countDownTimer <= 300f && !showTips) { //5min
            tipImageParent.SetActive(true);
            showTips = true;
            released = true;
        }
    }
}
