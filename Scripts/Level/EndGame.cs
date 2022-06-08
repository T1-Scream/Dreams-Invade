using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private RawImage video;
    [SerializeField] private VideoPlayer videoPlayer;
    private GameManager gameManager;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        videoPlayer.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            video.gameObject.SetActive(true);
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.Play();
            gameManager = FindObjectOfType<GameManager>();
            gameManager.MouseUnlocked();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        LastFrame();
    }

    private void Awake()
    {
        Canvas Canvas = FindObjectOfType<Canvas>();
        GameObject image = Canvas.transform.Find("videoImage").gameObject;
        video = image.GetComponent<RawImage>();
        video.gameObject.SetActive(false);
    }

    private void LastFrame()
    {
        long curFrame = videoPlayer.frame;
        long frameCount = Convert.ToInt64(videoPlayer.frameCount);

        if (curFrame > frameCount - 2)
            SceneManager.LoadScene("Menu");
    }
}
