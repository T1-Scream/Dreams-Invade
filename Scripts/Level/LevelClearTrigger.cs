using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System;

public class LevelClearTrigger : MonoBehaviour
{
    private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.LevelClearUI.SetActive(true);
            gameManager.startTimer = !gameManager.startTimer;
            FindObjectOfType<AudioManager>().Stop("Chase");
            FindObjectOfType<KillerMovement>().gameObject.SetActive(false);
            gameManager.MouseUnlocked();
        }
    }

}
