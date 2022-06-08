using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using QuantumTek.EncryptedSave;

public class RandomGamesManager : MonoBehaviour
{
    [SerializeField] private GameObject[] gameManager;
    [SerializeField] private GameObject[] gameObstacle;

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
        SetEventActive();
    }
    
    private void SetEventActive()
    {
        if (ES_Save.Exists("masterDrawIndex")) {
            int drawIndex = ES_Save.Load<int>("masterDrawIndex");

            if (drawIndex == 0) {
                gameManager[0].SetActive(true);
                gameObstacle[0].SetActive(true);
            }
            else if (drawIndex == 1) {
                gameManager[1].SetActive(true);
                gameObstacle[1].SetActive(true);
            }
            else if (drawIndex == 2) {
                gameManager[2].SetActive(true);
                gameObstacle[2].SetActive(true);
            }
        }
    }
}
