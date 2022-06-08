using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using QuantumTek.EncryptedSave;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Gameplay")]
    public bool ShowTimerUI;
    public bool startTimer;
    public float countDownTime = 600;
    public Text countDownText;
    public GameObject crossHair = null;
    public GameObject GameOverUI;
    public GameObject LevelClearUI;
    public GameObject GamePauseUI;
    public TMP_Text tipsText;

    [HideInInspector] public int nextSceneLoad;
    [HideInInspector] public float sensitivityData;

    [Header("Last Scene")]
    public bool ShowClearUI;

    [Header("LevelClearPanel")]
    [SerializeField] private TMP_Text nextLevelText;
    [SerializeField] private TMP_Text menuBtnText;
    [Header("GameOverPanel")]
    [SerializeField] private TMP_Text restartBtnText;
    [SerializeField] private TMP_Text menuBtnText2;

    private bool runOnceTime;
    private bool timeIsZero = false;

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
        BeginGame();
        ShowUI();
        LoadData();
        tipsText.text = string.Empty;
    }

    private void FixedUpdate()
    {
       if (countDownTime <= 0 && !timeIsZero) {
            GameOverUI.SetActive(true);
            Time.timeScale = 0;
            countDownTime = 600f;
            MouseUnlocked();
            timeIsZero = true;
        }
    }

    /////////////////////*UI*/////////////////////////

    private void CountDownTimer()
    {
        if (countDownTime > 0)
            countDownTime -= Time.deltaTime;
        else
            countDownTime = 0;

        DisplayTime(countDownTime);
    }
    
    private void DisplayTime(float time)
    {
        if (time < 0)
            time = 0;

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        countDownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void ShowUI()
    {
        if (!ShowTimerUI) {
            startTimer = false;
            countDownText.gameObject.SetActive(false);
        }
        else {
            startTimer = true;
            countDownText.gameObject.SetActive(true);
        }
    }

    private void LoadData()
    {
        sensitivityData = ES_Save.Load<float>("masterSen");
    }

    private void Update()
    {
        if (startTimer)
            CountDownTimer();

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    public void MouseLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MouseUnlocked()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BeginGame()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        tipsText.text = string.Empty;
        MouseLocked();

        int language = ES_Save.Load<int>("masterLanguage");
        if (language == 0)
             Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Chinese");
        else
             Lean.Localization.LeanLocalization.SetCurrentLanguageAll("English");

        countDownTime = 600f;
        timeIsZero = false;
        runOnceTime = false;
        if (ShowTimerUI) {
            GameOverUI.SetActive(false);
            GamePauseUI.SetActive(false);

            nextLevelText.text = Lean.Localization.LeanLocalization.GetTranslationText("NextLevel");
            menuBtnText.text = Lean.Localization.LeanLocalization.GetTranslationText("Menu");

            restartBtnText.text = Lean.Localization.LeanLocalization.GetTranslationText("Restart");
            menuBtnText2.text = Lean.Localization.LeanLocalization.GetTranslationText("Menu");
        }

            //------------------for last level scene----------------------
            if (ShowClearUI)
                LevelClearUI.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        GamePauseUI.SetActive(true);
        MouseUnlocked();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        GamePauseUI.SetActive(false);
        MouseLocked();
    }

    public void LevelClear()
    {
        if (nextSceneLoad > ES_Save.Load<int>("masterLevel"))
            ES_Save.Save(nextSceneLoad, "masterLevel");
    }

    public void GameOver(bool dead)
    {
        if (dead) {
            dead = false;
            GameOverUI.SetActive(true);
            Time.timeScale = 0;
            crossHair.SetActive(false);
            MouseUnlocked();
        }
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        LoadScene("Menu");
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
}
