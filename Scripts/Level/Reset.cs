using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Reset : MonoBehaviour
{
    public GameObject player;
    public GameObject killerAI;

    private PlayerMovement movement;
    private TMP_Text tipsText;

    private void OnTriggerEnter(Collider other)
    {
        movement = player.GetComponent<PlayerMovement>();
        movement.canJump = !movement.canJump;
        movement.Speed = 2f;
        movement.canCrouch = true;
        killerAI.SetActive(true);
        tipsText = GameObject.Find("TipsText").GetComponent<TMP_Text>();
        tipsText.fontSize = 45;
        tipsText.alignment = TextAlignmentOptions.Left;

        if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
            FindObjectOfType<PlayerCameraEvent>().SwitchGameTips("Find");
        else {
            FindObjectOfType<PlayerCameraEvent>().SwitchGameTips("Destroy");
            FindObjectOfType<PlayerCameraEvent>().canDestroy = true;
        }
    }
}
