using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using QuantumTek.EncryptedSave;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Text brightnessText = null;
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private float defaultBrightness = 1.0f;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private Resolution resolution;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text mainVolumeText = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text menuVolumeText = null;
    [SerializeField] private Slider menuVolumeSlider = null;
    [SerializeField] private TMP_Text effectVolumeText = null;
    [SerializeField] private Slider effectVolumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Controls Settings")]
    [SerializeField] private TMP_Text mouseSensText = null;
    [SerializeField] private Slider mouseSensSlider = null;
    [SerializeField] private float defaultSens = 5.0f;
    public float mouseSenitivity = 5.0f;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYAxisToggle = null;

    [Header("Level Selection Button")]
    [SerializeField] private Button[] levelButton = null;

    [Header("Key binding")]
    [SerializeField] private GameObject[] keyButton = null;
    [SerializeField] private KeyCode[] key;
    [SerializeField] private TMP_Text[] keyText = null;
    [SerializeField] private string[] savePath = null;
    [SerializeField] private KeyCode[] resetKey;

    [Header("Object")]
    [SerializeField] private GameObject messageBox = null;
    [SerializeField] private GameObject tutorialBtn = null;

    private int curLanguage;
    private int curResolution;
    private int tutorial;
    private int qualityLevel;
    private bool isFullScreen;
    private float brightnessLevel;
    private int keyCodeCount;

    private void Start()
    {
        StartResolution();
        LevelSelection();
        UpdateKeyCode();
        OriginKeyText();
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
        BeginMenu();
    }

    private void LevelSelection()
    {
        int levelAt = ES_Save.Load<int>("masterLevel");

        for (int i = 0; i < levelButton.Length; i++) {
            if (i + 2 > levelAt)
                levelButton[i].interactable = false;
        }

        levelButton[0].interactable = true;
    }

    private void UpdateKeyCode()
    {
        for (int i = 0; i < keyButton.Length; i++) {
            key[i] = keyButton[i].GetComponent<KeyMapButton>().keyInput;
        }
    }

    private void OriginKeyText()
    {
        for (int i = 0; i < keyButton.Length; i++) {
            keyText[i] = keyButton[i].GetComponentInChildren<TMP_Text>(); 
        }
    }

    public void ResetData()
    {
        ES_Save.Save(1, "masterLevel");
    }

    //Delete save data
    public void DeleteSaveData()
    {
        if (ES_Save.Exists("masterLevel"))
            ES_Save.DeleteData("masterLevel");
    }

    ////////////////*General Settings*///////////////
    public void SetLenguage(int languageIndex)
    {
        curLanguage = languageIndex;

        if (curLanguage == 0) {
            Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Chinese");
            ES_Save.Save(curLanguage, "masterLanguage");
        }
        else if (curLanguage == 1) {
            Lean.Localization.LeanLocalization.SetCurrentLanguageAll("English");
            ES_Save.Save(curLanguage, "masterLanguage");
        }
    }

    public void SetShowBeginnerTutorial(int modeIndex)
    {
        tutorial = modeIndex;

        if (tutorial == 0) {
            tutorialBtn.SetActive(true);
            ES_Save.Save(tutorial, "masterTutorial");
        }
        else if (tutorial == 1) {
            tutorialBtn.SetActive(false);
            ES_Save.Save(tutorial, "masterTutorial");
        }
    }

    /////////////////////////////////////////////////
    ///////////////*Graphics Settings*///////////////

    private void StartResolution()
    {
        if (resolutionDropdown != null) {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int curResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++) {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) {
                    curResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = ES_Save.Load<int>("masterResolution");
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        curResolution = resolutionIndex; 
    }

    public void SetBrightness(float brightness)
    {
        brightnessLevel = brightness;
        brightnessText.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool fullscreen)
    {
        isFullScreen = fullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        ES_Save.Save(brightnessLevel, "masterBrightness");

        ES_Save.Save(qualityLevel, "masterQuality");
        QualitySettings.SetQualityLevel(qualityLevel);

        ES_Save.Save(isFullScreen ? 1 : 0, "masterFullscreen");

        Screen.fullScreen = isFullScreen;

        resolution = resolutions[curResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        ES_Save.Save(curResolution, "masterResolution");
    }
    /////////////////////////////////////////////////
    ////////////////*Volume Settings*////////////////

    public void SetMainVolume(float volume)
    {
        AudioListener.volume = volume;
        mainVolumeText.text = volume.ToString("0%");
    }

    public void SetAudioVolume(string clip, float volume)
    {
        FindObjectOfType<AudioManager>().SetVolume(clip, volume);
    }

    public void SetMenuVolume(float volume)
    {
        volume = menuVolumeSlider.value;
        //audioManager.SetVolume("MenuBGM", volume);
        menuVolumeText.text = volume.ToString("0%");
    }

    public void SetEffectVolume(float volume)
    {
        volume = effectVolumeSlider.value;
        SetAudioVolume("PressButton", volume);
        SetAudioVolume("OpenDoor", volume);
        SetAudioVolume("CloseDoor", volume);
        SetAudioVolume("Elevator", volume);
        SetAudioVolume("Stare30", volume);
        SetAudioVolume("Stare60", volume);
        SetAudioVolume("Stare90", volume);
        SetAudioVolume("Chase", volume);
        SetAudioVolume("Scan", volume);
        SetAudioVolume("AutoDoor", volume);
        SetAudioVolume("Draw", volume);
        effectVolumeText.text = volume.ToString("0%");
    }

    public void VolumeApply()
    {
        ES_Save.Save(AudioListener.volume, "masterVolume");
        ES_Save.Save(menuVolumeSlider.value, "masterMenuVolume");
        ES_Save.Save(effectVolumeSlider.value, "masterEffectVolume");
    }

    public void ResetDefaults(string MenuType)
    {
        if (MenuType == "Graphics") {
            brightnessSlider.value = defaultBrightness;
            brightnessText.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1; //Medium
            QualitySettings.SetQualityLevel(1);

            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution curResolution = Screen.currentResolution;
            Screen.SetResolution(curResolution.width, curResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

        if (MenuType == "Audio") {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            menuVolumeSlider.value = defaultVolume;
            effectVolumeSlider.value = defaultVolume;
            mainVolumeText.text = defaultVolume.ToString("0%");
            VolumeApply();
        }

        if (MenuType == "Controls") {
            mouseSensSlider.value = defaultSens;
            mouseSenitivity = defaultSens;
            mouseSensText.text = defaultSens.ToString("0.0");
            invertYAxisToggle.isOn = false;
            for (int i = 0; i < resetKey.Length; i++) {
                key[i] = keyButton[i].GetComponent<KeyMapButton>().keyInput = resetKey[i];
                keyText[i].text = key[i].ToString();
            }
            ControlsApply();
        }
    }

    /////////////////////////////////////////////////
    ///////////////*Controls Settings*///////////////

    private void FirstTimeSettings()
    {
        if (!ES_Save.Exists("masterLanguage"))
            ES_Save.Save(0, "masterLanguage");

        if (!ES_Save.Exists("masterResolution"))
            ES_Save.Save(8, "masterResolution");

        if (!ES_Save.Exists("masterLevel"))
            ES_Save.Save(1, "masterLevel");

        if (!ES_Save.Exists("masterVolume"))
            ES_Save.Save(1, "masterVolume");

        if (!ES_Save.Exists("masterMenuVolume"))
            ES_Save.Save(1, "masterMenuVolume");

        if (!ES_Save.Exists("masterEffectVolume"))
            ES_Save.Save(1, "masterEffectVolume");

        if (!ES_Save.Exists("masterSen"))
            ES_Save.Save(mouseSenitivity, "masterSen");

        UpdateKeyCode();

        for (int i = 0; i < savePath.Length; i++) {
            if (!ES_Save.Exists(savePath[i]))
                ES_Save.Save(key[i].ToString(), savePath[i]);
        }
    }
    
    private void CompareKeyIsNotConflict()
    {
        for (int i = 0; i < key.Length; i++) {
            for (int j = 0; j < key.Length; j++) {
                if (key[i] == key[j])
                    keyCodeCount++;
            }
        }

        if (keyCodeCount == key.Length) {
            for (int s = 0; s < savePath.Length; s++) {
                ES_Save.Save(key[s].ToString(), savePath[s]);
            }
        }
        else
            messageBox.SetActive(true);

        keyCodeCount = 0; //reset
    }
    
    public void SetMouseSensitivity(float sensitivity)
    {
        sensitivity = mouseSensSlider.value;
        mouseSenitivity = Mathf.Round(sensitivity);
        mouseSensText.text = sensitivity.ToString("0.0");
    }

    public void ControlsApply()
    {
        if (invertYAxisToggle.isOn)
            ES_Save.Save(1, "masterInvertY-Axis");
        else
            ES_Save.Save(0, "masterInvertY-Axis");

        UpdateKeyCode();

        ES_Save.Save(mouseSenitivity, "masterSen");

        CompareKeyIsNotConflict();
    }

    /////////////////////////////////////////////////

    private void BeginMenu()
    {
        // mouse unlocked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        FirstTimeSettings();//set key if first time open
        Application.targetFrameRate = 60;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
