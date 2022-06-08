using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllMissionComplete : MonoBehaviour
{
    [SerializeField] private PuzzleVideoIsClear[] puzzleClearObj;
    [SerializeField] private GameObject button;

    private bool isVisible;

    private void Awake()
    {
        isVisible = false;
    }

    private bool IsAllMissionComplete()
    {
        for (int i = 0; i < puzzleClearObj.Length;i++) {
            if (puzzleClearObj[i].clear == false)
                return false;
        }

        return true;
    }

    private void Update()
    {
        if (IsAllMissionComplete() && !isVisible) {
            button.tag = "MissionButton";
            isVisible = true;
        }
    }

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
        button.tag = "Untagged";
    }
}
