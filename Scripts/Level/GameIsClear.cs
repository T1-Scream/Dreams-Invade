using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIsClear : MonoBehaviour
{
    public static GameIsClear instance;
    [SerializeField] private Animator wallAnimator;

    [HideInInspector] public static bool isClear = false;

    private PlayerCameraEvent player;
    private bool item1Destroyed;
    private bool item2Destroyed;
    private bool item3Destroyed;

    private void Awake()
    {
        player = FindObjectOfType<PlayerCameraEvent>();
    }

    private void Update()
    {
        item1Destroyed = player.item1Destroyed;
        item2Destroyed = player.item2Destroyed;
        item3Destroyed = player.item3Destroyed;

        if (item1Destroyed && item2Destroyed && item3Destroyed && !isClear) {
            wallAnimator.Play("LiftDoorOpen", 0, 0.0f);
            player.SwitchGameTips("Escape");
            isClear = true;
        }
    }
}
