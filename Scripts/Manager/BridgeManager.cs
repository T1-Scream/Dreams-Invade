using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BridgeManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Header("Tutorial")]
    [SerializeField] private Transform teleportPoint;
    public bool tutorialMode;

    private PlayerMovement movement;
    private bool dead = false;

    private void OnTriggerEnter(Collider other)
    {
        GameOver();
    }

    private void Awake()
    {
        if (!tutorialMode) {
            movement = player.GetComponent<PlayerMovement>();
            movement.Speed = 3.5f;
            movement.canJump = true;
            movement.jumpForce = 4.3f;
        }
    }

    private void GameOver()
    {
        if (tutorialMode)
            player.transform.position = teleportPoint.position;
        else {
            dead = true;
            FindObjectOfType<GameManager>().GameOver(dead);
        }
    }
}
